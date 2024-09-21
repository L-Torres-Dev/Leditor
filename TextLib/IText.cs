using System;

namespace Leditor.TextLib
{
    public interface IText
    {
        public int Length { get; }
        public void AddText(char c, int index);
        public void AddText(string str, int index);
        public void RemoveText(int index);
        public void RemoveText(int start, int end);

    }
}
