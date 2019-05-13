using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Shrewkins.Test
{
    public class DebugSymbolTests
    {
        [Fact]
        public void Hello() {

            var metadata = new MetadataBuilder(0, 0, 0, 0);

            var location = Assembly.GetExecutingAssembly().Location;
            var pdbPath = Path.ChangeExtension(location, "pdb");
            
            using (var fileStream = new FileStream(pdbPath, FileMode.Open)) 
            {
                var readerProvider =
                    MetadataReaderProvider.FromPortablePdbStream(fileStream, MetadataStreamOptions.Default);

                var reader = readerProvider.GetMetadataReader(MetadataReaderOptions.ApplyWindowsRuntimeProjections, null);


                var debugDir = new DebugDirectoryBuilder();
                
                debugDir.AddEmbeddedPortablePdbEntry(null, 0);
                
                
                
                var blob = new BlobBuilder();
                
                var methodHandle = new MethodDefinitionHandle();
                
                var pdb = new PortablePdbBuilder(metadata, ImmutableArray<int>.Empty, methodHandle, null);

                var blobContentId = pdb.Serialize(blob);

                blob.ToArray();
                
            }

        }
        
    }
}