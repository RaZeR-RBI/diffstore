using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Diffstore.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

#if !CLS
[assembly: CLSCompliant(false)]
#else
[assembly: CLSCompliant(true)]
#endif