// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace PlugIn.HostDriver;

/// <summary>
/// Provides functionality to launch plug-in host processes, load plug-ins into that host, and obtain the host's
/// standard output.
/// </summary>
public sealed class PlugInHost : IDisposable
{
#if DEBUG
    private const string Configuration = "Debug";
#else
        const string Configuration = "Release";
#endif

    private readonly PlugInBuilder _plugInBuilder = new();

    /// <summary>
    /// Removes the temporary plug-in projects created by calls to
    /// <see cref="Run{TResult}(string, PlugInDescriptor, PlugInDescriptor, Func{Stream, Task{TResult}})"/>.
    /// </summary>
    public void Dispose()
    {
        _plugInBuilder.Dispose();
    }

    /// <summary>
    /// Launches a plug-in host process built for the specified target framework, loads two plug-ins into that host,
    /// and makes the host's standard output available to the caller.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result type. The <paramref name="stdOutStreamToResult"/> argument is supplied with the host's standard
    /// output, and returns a value of this type, which then becomes the result of this method.
    /// </typeparam>
    /// <param name="hostRuntimeTfm">
    /// <para>
    /// The target framework moniker (TFM) of the host runtime. For example, "net8.0" or "net48".
    /// </para>
    /// <para>
    /// This must match one of the target frameworks for which either the <c>PlugIn.HostDotnet</c> or the
    /// <c>PlugIn.HostNetFx</c> is built.
    /// </para>
    /// <para>
    /// Note that this does not necessarily define the runtime that the host uses. For .NET Framework TFMs, there is
    /// only one instance of .NET Framework 4.x installed, so that's the version the host will use. (That said, the
    /// specified TFM can cause the runtime to enable certain backwards-compatibility features.) With .NET TFMs, you
    /// will get the runtime you asked for if it is installed. However, if running on a system that only has newer
    /// versions of .NET installed than you asked for, you will get one of those instead.
    /// </para>
    /// </param>
    /// <param name="firstPlugIn"></param>
    /// <param name="secondPlugIn"></param>
    /// <param name="stdOutStreamToResult"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<TResult> Run<TResult>(
        string hostRuntimeTfm,
        PlugInDescriptor firstPlugIn,
        PlugInDescriptor secondPlugIn,
        Func<Stream, Task<TResult>> stdOutStreamToResult)
    {
        string launcher;

        if (hostRuntimeTfm.StartsWith("net"))
        {
            if (hostRuntimeTfm.Contains('.'))
            {
                // .NET Core or .NET 5+
                launcher = "PlugIn.HostDotnet";
            }
            else
            {
                // .NET Framework
                launcher = "PlugIn.HostNetFx";
            }
        }
        else
        {
            throw new ArgumentException($"Unsupported host runtime TFM: {hostRuntimeTfm}");
        }

        DirectoryInfo plugInsFolder = new(
            Path.Combine(AppContext.BaseDirectory, "../../../../"));
        if (!plugInsFolder.Exists)
        {
            throw new DirectoryNotFoundException($"PlugIns folder not found: {plugInsFolder.FullName}");
        }

        var plugInHostProjectFolder = Path.Combine(
            plugInsFolder.FullName,
            launcher);
        if (!Directory.Exists(plugInHostProjectFolder))
        {
            throw new DirectoryNotFoundException($"PlugIn host project folder not found at {plugInHostProjectFolder}");
        }
        var plugInHostExecutableFolder = Path.Combine(
            plugInHostProjectFolder,
            $"bin/{Configuration}/{hostRuntimeTfm}/");
        if (!Directory.Exists(plugInHostExecutableFolder))
        {
            throw new DirectoryNotFoundException($"PlugIn host build output folder not found at {plugInHostExecutableFolder}");
        }

        var plugInHostExecutablePath = Path.Combine(
            plugInHostExecutableFolder,
            $"{launcher}.exe");
        if (!File.Exists(plugInHostExecutablePath))
        {
            throw new FileNotFoundException($"PlugIn host executable not found at {plugInHostExecutablePath}");
        }

        var firstPlugInPath = await _plugInBuilder.GetPlugInDllPathAsync(firstPlugIn);
        var secondPlugInPath = await _plugInBuilder.GetPlugInDllPathAsync(secondPlugIn);

        var startInfo = new ProcessStartInfo
        {
            FileName = plugInHostExecutablePath,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            //CreateNoWindow = true,
            Arguments = $"{firstPlugInPath} {secondPlugInPath}",
            WorkingDirectory = plugInHostExecutableFolder,
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        // Pass the StandardOutput stream to the provided function
        var resultTask = stdOutStreamToResult(process.StandardOutput.BaseStream);
        var processTask = process.WaitForExitAsync();
        var firstToFinish = await Task.WhenAny(processTask, resultTask);

        if (process.HasExited && process.ExitCode != 0)
        {
            Console.WriteLine($"{plugInHostExecutablePath} exited with code {process.ExitCode} for args {startInfo.Arguments}");
        }

        if (!resultTask.IsCompleted)
        {
            // The process finished, but the result task is still running. It's possible that
            // it is nearly done, so give it some time.
            await Task.WhenAny(resultTask, Task.Delay(2000));
        }

        if (!resultTask.IsCompleted)
        {
            throw new InvalidOperationException("Did not get output from program");
        }
        var result = await resultTask;

        return result;
    }
}
