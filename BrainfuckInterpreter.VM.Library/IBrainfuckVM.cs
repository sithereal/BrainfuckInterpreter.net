namespace BrainfuckInterpreter.VM.Library
{
    public delegate void OutputEventHandler(byte c);
    public interface IBrainfuckVM
    {
        event OutputEventHandler Output;
        byte[] Buffer { get; }
        int DI { get; }

        int PI { get; }

        string OutputBuffer { get; }

        void Reset();
        void Process(char symbol);
        void MoveForward();
        void Move(int pos);
        void MoveBackward();
        byte Get();
        void Increase();
        void Decrease();

        void DoOutput();
        void DoInput(byte c);
        void WhileBegin();
        void WhileEnd();

        void Run(string program);
    }
}