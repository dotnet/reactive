using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Tests.System.Reactive.ApiApprovals, PublicKey=00240000048000009400000006020000002400005253413100040000010001008f5cff058631087031f8350f30a36fa078027e5df2316b564352dc9eb7af7ce856016d3c5e9d058036fe73bb5c83987bd3fc0793fbe25d633cc4f37c2bd5f1d717cd2a81661bec08f0971dc6078e17bde372b89005e7738a0ebd501b896ca3e8315270ff64df7809dd912c372df61785a5085b3553b7872e39b1b1cc0ff5a6bc")]

namespace System.Reactive
{
    /// <summary>
    /// A type so that we can have at least one non-forwarded type in this assembly.
    /// This is because the API Approval tests seem to do something quite odd with
    /// assembly references, meaning that unless we actually retrieve a real type
    /// object from the assembly we want to inspect, it won't be available.
    /// </summary>
    internal class Placeholder
    {
    }
}
