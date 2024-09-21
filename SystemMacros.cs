namespace Leditor
{
    public static class SystemMacros
    {
        // Extracts the low-order word (16 bits)
        public static ushort LOWORD(uint value)
        {
            return (ushort)(value & 0xFFFF);
        }

        // Extracts the high-order word (16 bits)
        public static ushort HIWORD(uint value)
        {
            return (ushort)((value >> 16) & 0xFFFF);
        }
    }
}
