module Fable.Tests.Local.Integration

open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Symbols
open FSharp.Compiler.Text
open System.IO
open Xunit
open FSharp.Compiler.SourceCodeServices
open Fable.Transforms.State
open Fable.Cli.Main
open Fable.Compiler.Util

let checker = FSharpChecker.Create(keepAssemblyContents = true)

let parseAndCheckSingleFile (input: string) =
    let file = Path.ChangeExtension(System.IO.Path.GetTempFileName(), "fsx")
    File.WriteAllText(file, input)
    // Get context representing a stand-alone (script) file
    let projOptions, _errors =
        checker.GetProjectOptionsFromScript(file, SourceText.ofString input, assumeDotNetFramework = false)
        |> Async.RunSynchronously

    checker.ParseAndCheckProject(projOptions) |> Async.RunSynchronously

let getDeclarations (input: string) =
    let checkProjectResults = parseAndCheckSingleFile input
    let checkedFile = checkProjectResults.AssemblyContents.ImplementationFiles.[0]

    checkedFile.Declarations
//let getDeclarations input =
//    input
//let checkProjectResults = parseAndCheckSingleFile input
//let checkedFile = checkProjectResults.AssemblyContents.ImplementationFiles.[0]

//checkedFile.Declarations
let getCompiler () =
    async {
        let cliArgs = Fable.Tests.Local.Compiler.Cached.cliArgs

        let projCracked =
            ProjectCracked.Init({ cliArgs with NoCache = true }, false, evaluateOnly = true)

        let checker = InteractiveChecker.Create(projCracked.ProjectOptions)
        let! assemblies = checker.GetImportedAssemblies()
        //Fable.Tests.Compiler.Util.
        let fableProj =
            Project.From(
                projCracked.ProjectFile,
                projCracked.ProjectOptions.SourceFiles,
                [],
                assemblies,
                Fable.Compiler.Util.Log.log,
                ?precompiledInfo = (projCracked.PrecompiledInfo |> Option.map (fun i -> i :> _)),
                getPlugin = Reflection.loadType projCracked.CliArgs
            )

        //return FableCompiler(checker, projCracked, fableProj)
        return projCracked.MakeCompiler("test.fs", fableProj, true)
    }

[<Fact>]
let ``My test`` () =
    let input =
        """
module Test

let x = 1 + 2
"""

    let declarations = getDeclarations input

    let currentFile = "test.fs"

    let fableProj =
        Fable.Transforms.State.Project.From("", [||], [], [], Fable.Compiler.Util.Log.log)

    let opts = Fable.CompilerOptionsHelper.Make()
    let checker = FSharpChecker.Create(keepAssemblyContents = true)
    //let compiler: Fable.Compiler =
    //    Fable.Transforms.State.CompilerImpl(
    //        currentFile,
    //        fableProj,
    //        opts,
    //        fableLibDir,
    //        crackerResponse.OutputType,
    //        ?outDir = cliArgs.OutDir
    //    )
    let compiler = getCompiler () |> Async.RunSynchronously
    //:> Fable.Compiler

    let compilerInterface = compiler :> Fable.Compiler

    let response =
        Fable.Transforms.FSharp2Fable.Compiler.transformFileDeclarations compiler declarations

    Assert.True(true)
