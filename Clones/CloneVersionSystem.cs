using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private List<Clone> clonesList = new List<Clone>();
    public string Execute(string query)
    {
        if (clonesList.Count == 0) { clonesList.Add(new Clone()); }

        var commandArr = query.Split(' ');
        var command = commandArr[0];
        var cloneIndex = Convert.ToInt32(commandArr[1]) - 1;
        if (command == "learn") clonesList[cloneIndex].Learn(commandArr[2]);
        if (command == "rollback") clonesList[cloneIndex].Rollback();
        if (command == "relearn") clonesList[cloneIndex].Relearn();
        if (command == "clone") clonesList.Add(new Clone(clonesList[cloneIndex]));
        if (command == "check") return clonesList[cloneIndex].Check();
        return null;
    }

    public class Clone
    {
        public ItemsStack<string> ExistsProgram;
        public ItemsStack<string> RemovedPrograms;

        public Clone()
        {
            ExistsProgram = new ItemsStack<string>();
            RemovedPrograms = new ItemsStack<string>();
        }

        public Clone(Clone clonedProgram)
        {
            ExistsProgram = new ItemsStack<string>()
            { Head = clonedProgram.ExistsProgram.Head, Last = clonedProgram.ExistsProgram.Last };
            RemovedPrograms = new ItemsStack<string>()
            { Head = clonedProgram.RemovedPrograms.Head, Last = clonedProgram.RemovedPrograms.Last };
        }

        public void Learn(string program)
        {
            ExistsProgram.Push(program);
            RemovedPrograms = new ItemsStack<string>();
        }

        public void Rollback()
        {
            RemovedPrograms.Push(ExistsProgram.Pop());
        }

        public void Relearn()
        {
            ExistsProgram.Push(RemovedPrograms.Pop());
        }

        public string Check()
        {
            if (ExistsProgram.Head == null)
                return "basic";

            var temp = ExistsProgram.Pop();
            ExistsProgram.Push(temp);

            return temp;
        }
    }
}

public class Items<T>
{
    public T Value { get; set; }
    public Items<T> Next { get; set; }
    public Items<T> Previous { get; set; }
}

public class ItemsStack<T>
{
    public Items<T> Head;
    public Items<T> Last;


    public void Push(T item)
    {
        if (Head == null)
            Last = Head = new Items<T> { Value = item, Next = null, Previous = null };
        else
        {
            var newItem = new Items<T> { Value = item, Next = null, Previous = Last };
            Last.Next = newItem;
            Last = newItem;
        }
    }

    public T Pop()
    {
        if (Last == null) return default(T);
        var result = Last.Value;
        Last = Last.Previous;
        if (Last == null)
            Head = null;
        else
            Last.Next = null;
        return result;
    }
}