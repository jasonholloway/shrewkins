using System;
using System.Collections.Generic;

namespace Shrewkins
{
    public static class OpWriter 
    {
        public static void Write(Span<byte> @out, IEnumerable<Instruction> els) {
            foreach (var el in els) {
                switch (el) {
                    case BasicInstruction ins:
                        var op = ins.Op;
                        switch (op.Size) {
                            case 1:
                                @out = Encode8(@out, op.Value);
                                break;
                            case 2:
                                @out = Encode8(@out, 0xFE);
                                @out = Encode8(@out, op.Value);
                                break;
                        }

                        var operand = ins.Operand;
                        switch (operand.Size) {
                            case 0: break;
                            case 1:
                                @out = Encode8(@out, operand.Raw);
                                break;
                            case 2:
                                @out = Encode16(@out, operand.Raw);
                                break;
                            case 4:
                                @out = Encode32(@out, operand.Raw);
                                break;
                            case 8:
                                @out = Encode64(@out, operand.Raw);
                                break;
                        }

                        break;
                }
            }
        }

        static Span<byte> Encode8(Span<byte> data, long val) {
            data[0] = (byte)val;
            return data.Slice(1);
        }
        
        static Span<byte> Encode16(Span<byte> data, long val) {
            if (!BitConverter.TryWriteBytes(data, (short)val)) throw new InvalidOperationException("Insufficient space to write!");
            return data.Slice(2);
        }
        
        static Span<byte> Encode32(Span<byte> data, long val) {
            if (!BitConverter.TryWriteBytes(data, (int)val)) throw new InvalidOperationException("Insufficient space to write!");
            return data.Slice(4);
        }
        
        static Span<byte> Encode64(Span<byte> data, long val) {
            if (!BitConverter.TryWriteBytes(data, val)) throw new InvalidOperationException("Insufficient space to write!");
            return data.Slice(8);
        }
    }
}