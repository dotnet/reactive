namespace RxGauntlet.Build;

public record BuildOutput(
    int BuildProcessExitCode,
    string OutputFolder,
    string BuildStdOut)
{
    public bool BuildSucceeded => BuildProcessExitCode == 0;
}

public record BuildAndRunOutput(
    int BuildProcessExitCode,
    string OutputFolder,
    string BuildStdOut,
    int? ExecuteExitCode,
    string? ExecuteStdOut,
    string? ExecuteStdErr) : BuildOutput(BuildProcessExitCode, OutputFolder, BuildStdOut);
