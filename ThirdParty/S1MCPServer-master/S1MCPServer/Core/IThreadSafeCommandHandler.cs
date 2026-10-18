namespace S1MCPServer.Core;

/// <summary>
/// Marker interface for command handlers that never touch the Unity API and keep no
/// mutable shared state without locking.
///
/// Such handlers are safe to run directly on the TCP server thread. That is what lets a
/// client complete its handshake, list methods and query <c>status</c> while the game
/// sits paused in the main menu and the main-thread command queue is not being drained.
///
/// Everything else must go through <see cref="CommandQueue"/> and be executed on the
/// Unity main thread.
/// </summary>
public interface IThreadSafeCommandHandler : ICommandHandler
{
}
