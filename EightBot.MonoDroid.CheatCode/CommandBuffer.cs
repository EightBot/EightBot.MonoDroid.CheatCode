using System;
using System.Collections.Generic;
using System.Linq;

namespace EightBot.MonoDroid.CheatCode
{
    internal class CommandBuffer<T>
    {
        private readonly object _queueLock = new object();

        private LinkedList<T> Buffer { get; set; }

        public int Size { get; private set; }

        public T[] Commands
        {
            get
            {
                lock (_queueLock)
                {
                    return Buffer.ToArray();
                }
            }
        }

        public CommandBuffer(int size)
        {
            Size = size;

            Buffer = new LinkedList<T>();
        }

        public void Add(T command)
        {
            Buffer.AddLast(command);

            lock (_queueLock)
            {
                int overflow = Buffer.Count - Size;

                if (overflow > 0)
                    Remove(overflow);
            }
        }

        public void Remove(Int32 count)
        {
            lock (_queueLock)
            {
                while (Buffer.Count > Size)
                {
                    Buffer.RemoveFirst();
                }
            }
        }

        public void Clear()
        {
            lock (_queueLock)
                Buffer.Clear();
        }

        public Boolean ProcessCommandList(T[] commandList)
        {
            if (commandList == null)
                throw new ArgumentNullException("commandList");

            //Too little or too much to do anything with
            if (commandList.Length == 0 || commandList.Length > Size)
                return false;

            bool matchSuccessful = false;

            lock (_queueLock)
                matchSuccessful = commandList.EndSequenceEqual(Commands);

            if (matchSuccessful)
                Clear();

            return matchSuccessful;
        }
    }
}