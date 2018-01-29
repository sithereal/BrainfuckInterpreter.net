using System;

namespace BrainfuckInterpreter.VM.Library
{
    public class BrainfuckVM : IBrainfuckVM
    {
        private byte[] buffer;
        public byte[] Buffer => buffer;

        private int di;
        private int pi;
        private string program;
        string outputBuffer;

        public event OutputEventHandler Output;

        public BrainfuckVM()
        {
            buffer = new byte[30000];
        }

        public int DI => di;

        public int PI => pi;

        public string OutputBuffer => outputBuffer;

        public void Reset()
        {
            di = 0;
            pi = 0;
            program = "";
            outputBuffer = "";
            Output = null;
            Array.Clear(buffer, 0, buffer.Length);
        }
        public void Run(string program)
        {
            this.program = program;
            pi = 0;
            while(pi!=program.Length)
            {
                char instruction = program[pi];
                Process(instruction);
                pi++;
            }
        }
        public void Process(char c)
        {
            switch(c)
            {
                case '<':
                    MoveForward();
                    break;
                case '>':
                    MoveBackward();
                    break;
                case '+':
                    Increase();
                    break;
                case '-':
                    Decrease();
                    break;
                case '.':
                    DoOutput();
                    break;
                case ',':
                    DoInput(0); // !!!
                    break;
                case '[':
                    WhileBegin();
                    break;
                case ']':
                    WhileEnd();
                    break;
                case '\n':
                case ' ':
                    break;
                default:
                    throw new InvalidOperandException(c.ToString());
            }
        }

        public void MoveForward()
        {
            if (di == buffer.Length-1) di = 0;
            else di++;            
        }

        public void MoveBackward()
        {
            if (di == 0) di = buffer.Length-1;
            else di--;            
        }

        public void Move(int pos)
        {           
            di = pos;            
        }

        public byte Get()
        {
            return buffer[di];
        }

        public void Increase()
        {
            buffer[di]++;
        }

        public void Decrease()
        {
            buffer[di]--;
        }

        public void DoOutput()
        {
            byte b = Get();
            outputBuffer += Convert.ToChar(b);
            Output?.Invoke(b);
        }

        public void DoInput(byte c)
        {
            buffer[di] = c;
        }

        public void WhileBegin()
        {
            if (string.IsNullOrEmpty(program)) return;
            if (buffer[di] == 0)
            {
                int loop = 1;
                while (loop > 0)
                {
                    pi++;
                    char c = program[pi];
                    if (c == '[')
                    {
                        loop++;
                    }
                    else
                    if (c == ']')
                    {
                        loop--;
                    }
                }
            }
        }

        public void WhileEnd()
        {
            if (string.IsNullOrEmpty(program)) return;
            int loop = 1;
            while (loop > 0)
            {
                pi--;
                char c = program[pi];
                if (c == '[')
                {
                    loop--;
                }
                else if (c == ']')
                {
                    loop++;
                }
            }
            pi--;
        }
    }
}