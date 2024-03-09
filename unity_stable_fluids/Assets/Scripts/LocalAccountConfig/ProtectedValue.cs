

    public struct PLong
    {
        long m_Value;
        long m_Check;

        public PLong(long v)
    {
            m_Value = PLong.Encode(v);
            m_Check = PLong.Check(v);
        }

        public static implicit operator PLong(long value)
        {
            return new PLong(value);
        }

        public static implicit operator long(PLong p)
        {
            long v = PLong.Decode(p.m_Value);
            if (p.m_Check == PLong.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PLong.Decode(m_Value).ToString();
        }

        static long Encode(long v)
        {
            return v ^ 0x5555555555555555;
        }

        static long Decode(long v)
        {
            return v ^ 0x5555555555555555;
        }

        static long Check(long v)
        {
            return v ^ 0x7777777777777777;
        }
    }

    public struct PInt
    {
        int m_Value;
        int m_Check;

        public PInt(int v)
        {
            m_Value = PInt.Encode(v);
            m_Check = PInt.Check(v);
        }

        public static implicit operator PInt(int value)
        {
            return new PInt(value);
        }

        public static implicit operator int(PInt p)
        {
            int v = PInt.Decode(p.m_Value);
            if (p.m_Check == PInt.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PInt.Decode(m_Value).ToString();
        }

        static int Encode(int v)
        {
            return v ^ 0x55555555;
        }

        static int Decode(int v)
        {
            return v ^ 0x55555555;
        }

        static int Check(int v)
        {
            return v ^ 0x77777777;
        }
    }

    public struct PUInt
    {
        uint m_Value;
        uint m_Check;

        public PUInt(uint v)
        {
            m_Value = PUInt.Encode(v);
            m_Check = PUInt.Check(v);
        }

        public static implicit operator PUInt(uint value)
        {
            return new PUInt(value);
        }

        public static implicit operator uint(PUInt p)
        {
            uint v = PUInt.Decode(p.m_Value);
            if (p.m_Check == PUInt.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PUInt.Decode(m_Value).ToString();
        }

        static uint Encode(uint v)
        {
            return v ^ 0x55555555;
        }

        static uint Decode(uint v)
        {
            return v ^ 0x55555555;
        }

        static uint Check(uint v)
        {
            return v ^ 0x77777777;
        }
    }

    public struct PShort
    {
        short m_Value;
        short m_Check;

        public PShort(short v)
        {
            m_Value = PShort.Encode(v);
            m_Check = PShort.Check(v);
        }

        public static implicit operator PShort(short value)
        {
            return new PShort(value);
        }

        public static implicit operator short(PShort p)
        {
            short v = PShort.Decode(p.m_Value);
            if (p.m_Check == PShort.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PShort.Decode(m_Value).ToString();
        }

        static short Encode(short v)
        {
            return (short)(v ^ 0x5555);
        }

        static short Decode(short v)
        {
            return (short)(v ^ 0x5555);
        }

        static short Check(short v)
        {
            return (short)(v ^ 0x7777);
        }
    }

    public struct PUShort
    {
        ushort m_Value;
        ushort m_Check;

        public PUShort(ushort v)
        {
            m_Value = PUShort.Encode(v);
            m_Check = PUShort.Check(v);
        }

        public static implicit operator PUShort(ushort value)
        {
            return new PUShort(value);
        }

        public static implicit operator ushort(PUShort p)
        {
            ushort v = PUShort.Decode(p.m_Value);
            if (p.m_Check == PUShort.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PUShort.Decode(m_Value).ToString();
        }

        static ushort Encode(ushort v)
        {
            return (ushort)(v ^ 0x5555);
        }

        static ushort Decode(ushort v)
        {
            return (ushort)(v ^ 0x5555);
        }

        static ushort Check(ushort v)
        {
            return (ushort)(v ^ 0x7777);
        }
    }

    public struct PSByte
    {
        sbyte m_Value;
        sbyte m_Check;

        public PSByte(sbyte v)
        {
            m_Value = PSByte.Encode(v);
            m_Check = PSByte.Check(v);
        }

        public static implicit operator PSByte(sbyte value)
        {
            return new PSByte(value);
        }

        public static implicit operator sbyte(PSByte p)
        {
            sbyte v = PSByte.Decode(p.m_Value);
            if (p.m_Check == PSByte.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PSByte.Decode(m_Value).ToString();
        }

        static sbyte Encode(sbyte v)
        {
            return (sbyte)(v ^ 0x55);
        }

        static sbyte Decode(sbyte v)
        {
            return (sbyte)(v ^ 0x55);
        }

        static sbyte Check(sbyte v)
        {
            return (sbyte)(v ^ 0x77);
        }
    }

    public struct PByte
    {
        byte m_Value;
        byte m_Check;

        public PByte(byte v)
        {
            m_Value = PByte.Encode(v);
            m_Check = PByte.Check(v);
        }

        public static implicit operator PByte(byte value)
        {
            return new PByte(value);
        }

        public static implicit operator byte(PByte p)
        {
            byte v = PByte.Decode(p.m_Value);
            if (p.m_Check == PByte.Check(v))
                return v;
            else
                return 0;
        }

        public override string ToString()
        {
            return PByte.Decode(m_Value).ToString();
        }

        static byte Encode(byte v)
        {
            return (byte)(v ^ 0x55);
        }

        static byte Decode(byte v)
        {
            return (byte)(v ^ 0x55);
        }

        static byte Check(byte v)
        {
            return (byte)(v ^ 0x77);
        }
    }

