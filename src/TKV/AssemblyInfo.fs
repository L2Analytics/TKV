namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("TKV")>]
[<assembly: AssemblyProductAttribute("TKV")>]
[<assembly: AssemblyDescriptionAttribute("Time-Key-Value")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
