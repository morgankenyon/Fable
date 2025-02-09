module Fable.Tests.Local.Compiler

open System
open Fable
open Fable.Cli.Main
open Fable.Transforms.State
open Fable.Compiler.Util


type Result = LogEntry list

module Result =
    let errors = List.filter (fun (m: LogEntry) -> m.Severity = Severity.Error)
    let warnings = List.filter (fun (m: LogEntry) -> m.Severity = Severity.Warning)
    let wasFailure = errors >> List.isEmpty >> not

type Settings = { Opens: string list }

module Settings =
    let standard = { Opens = [ "System" ] }

/// NOTE: NOT threadsafe
///       -> don't use `parallel`
module Cached =
    let projDir =
        IO.Path.Join(__SOURCE_DIRECTORY__, "../TestProject") |> Path.normalizeFullPath

    let projFile = IO.Path.Join(projDir, "TestProject.fsproj") |> Path.normalizeFullPath
    let sourceFile = IO.Path.Join(projDir, "Program.fs") |> Path.normalizeFullPath

    let cliArgs =
        let compilerOptions = CompilerOptionsHelper.Make()

        {
            CliArgs.ProjectFile = projFile
            FableLibraryPath = None
            RootDir = projDir
            Configuration = "Debug"
            OutDir = None
            IsWatch = false
            Precompile = false
            PrecompiledLib = None
            PrintAst = false
            SourceMaps = false
            SourceMapsRoot = None
            NoRestore = false
            NoCache = false
            NoParallelTypeCheck = false
            Exclude = [ "Fable.Core" ]
            Replace = Map.empty
            RunProcess = None
            CompilerOptions = compilerOptions
            Verbosity = Verbosity.Normal
        }

    let mutable private state = State.Create(cliArgs, recompileAllFiles = true)
