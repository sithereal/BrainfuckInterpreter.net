using NUnit.Framework;
using BrainfuckInterpreter.VM.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter.VM.Library.Tests
{
    [TestFixture()]
    public class BrainfuckVMTests
    {
        IBrainfuckVM _vm;
        IBrainfuckVM vm
        {
            get
            {
                if (_vm == null) _vm = new BrainfuckVM();
                return _vm;
            }
        }
        [Test()]
        public void VM_Create()
        {            
            Assert.IsInstanceOf(typeof(BrainfuckVM), vm);
        }

        [Test()]
        public void VM_Init()
        {
            byte[] buffer = vm.Buffer;
            Assert.AreEqual(buffer.Length, 30000);
        }

        [Test()]
        public void VM_Reset()
        {
            vm.Reset();
            int pc = vm.DI;
            Assert.AreEqual(pc, 0);
        }

        [Test()]        
        public void VM_Accepts_Valid_Symbols(
            [Values('<', '>', '+', '-', '.', ',', '[', ']')]
            char symbol)
        {
            vm.Reset();          
            try
            {
                vm.Process(symbol);                
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test()]
        public void VM_MoveForward()
        {
            vm.Reset();
            int currentDI = vm.DI;
            vm.MoveForward();
            int expectedDI = (currentDI == vm.Buffer.Length-1 ? 0 : currentDI + 1);
            Assert.AreEqual(expectedDI, vm.DI);

            vm.Move(vm.Buffer.Length-1);
            vm.MoveForward();
            expectedDI = 0;
            Assert.AreEqual(vm.DI, expectedDI);
        }        

        [Test()]
        public void VM_MoveBackward()
        {
            int currentDI = vm.DI;
            vm.MoveBackward();
            int expectedDI = currentDI == 0 ? vm.Buffer.Length-1 : currentDI - 1;
            Assert.AreEqual(expectedDI, vm.DI);

            vm.Move(0);
            vm.MoveBackward();
            Assert.AreEqual(vm.DI, vm.Buffer.Length-1);
        }

        [Test()]
        public void VM_Increase()
        {            
            byte curVal = vm.Get();
            vm.Increase();
            byte expected = (byte)(curVal + 1);
            Assert.AreEqual(vm.Get(), expected);
        }
        [Test()]
        public void VM_Decrease()
        {
            byte curVal = vm.Get();
            vm.Decrease();
            byte expected = (byte)(curVal - 1);
            Assert.AreEqual(vm.Get(), expected);
        }
        [Test()]
        public void VM_Putchar()
        {
            vm.Reset();
            byte output = 0;
            OutputEventHandler oeh = (c) => output = c;
            vm.Output += oeh;            
            vm.Increase();
            vm.DoOutput();
            byte expectedOut = 1;
            Assert.AreEqual(output, expectedOut);
            vm.Output -= oeh;
        }

        [Test()]
        public void VM_Getchar()
        {
            byte c = 42;            
            vm.Reset();            
            vm.DoInput(c);
            byte expectedC= vm.Get();
            Assert.AreEqual(c, expectedC);            
        }


        [Test()]
      //  [Timeout(5000)]
        public void VM_Run()
        {
            // Hello World!\n
            string program = "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
            vm.Reset();
            vm.Run(program);
            string output = vm.OutputBuffer;
            string expected = "Hello World!\n";
            Assert.AreEqual(expected, output);
            
        }

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
    }

}