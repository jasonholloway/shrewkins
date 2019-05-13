using System.Collections.Generic;

namespace Shrewkins.Test
{
    public interface ISource
    {
        IReadOnlyList<ITarget> Targets { get; }
    }
    
    public interface ITarget
    {
        IReadOnlyList<ISource> Sources { get; }
    }
    
    public interface IProgram : ITarget, ISource
    {
        IReadOnlyList<ISource> Sources { get; }
        IReadOnlyList<ITarget> Targets { get; }
    }
    
    
    public class Slice : IProgram
    {
        public IReadOnlyList<ISource> Sources { get; }
        public IReadOnlyList<ITarget> Targets { get; }

        public Slice() {
                
        }
    }

    public class Input : ISource
    {
        public IReadOnlyList<ITarget> Targets { get; }
    }

    public class Output : ITarget
    {
        public IReadOnlyList<ISource> Sources { get; }
    }

    
}