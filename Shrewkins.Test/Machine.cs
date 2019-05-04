using System;

namespace JsonLens.Compiler 
{
    
    public enum Mode: byte {
        Dump,
        ReadObject,
        MatchProps,
        ReadAll,
        ReadObjectEnd
    }
    
    public enum Signal : byte
    {
        Seek,
        SeekNext,
        Return,
        
        End,
        Underrun,
        BadInput,
        Ok
    }

    public enum Stream {
        Ops,
        In,
        Tokens,
        Out1,
        Out2
    }


    public struct Machine 
    {
        
        public (Signal, int) Next(ref Context x) 
        {
            Mode mode = Mode.Dump;
            
            start:
            switch (mode) 
            {
                case Mode.ReadAll:
                    
                    throw new Exception("HELLO");
                
                
                case Mode.Dump:
//                    _tokenizer.Next(ref @in, out var result);
                    throw new NotImplementedException();

                case Mode.ReadObject:
//                    var signal = _tokenizer.Next(ref @in, out var token);
//                    switch (signal) {
//                        case Signal.Ok:
//                            //push continuation
//                            _stack.Push(op);
//                           
//                            throw new NotImplementedException();
//                        
//                        default:
//                            return (signal, Stream.In);
//                    }
                    
                    throw new NotImplementedException();
                
                
                case Mode.ReadObjectEnd:
                    throw new NotImplementedException();
                    
                case Mode.MatchProps:
                    //props should be matched via a trie
                    //each prop encountered should be tested for a match
                    //problem is, how to structure program to fork
                    //easy - we always match multiple props
                    //the prefix trie yields an index, like a jump statement
                    throw new NotImplementedException();
            }
            
            throw new NotImplementedException();
        }
    }
}
