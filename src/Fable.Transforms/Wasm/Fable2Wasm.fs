module rec Fable.Transforms.Fable2Wasm

open Fable
open Fable.AST

type Context =
    {
        File: Fable.File
    //UsedNames: UsedNames
    //BoundVars: BoundVars
    //DecisionTargets: (Fable.Ident list * Fable.Expr) list
    //HoistVars: Fable.Ident list -> bool
    //TailCallOpportunity: ITailCallOpportunity option
    //OptimizeTailCall: unit -> unit
    //ScopedTypeParams: Set<string>
    //TypeParamsScope: int
    }

type IWasmCompiler =
    inherit Compiler
    abstract GetIdentifier: ctx: Context * name: string -> string

module Util =
    let getIdentifier (_com: IWasmCompiler) (_ctx: Context) (name: string) = name

    let rec transformDeclaration (com: IWasmCompiler) ctx (dec: Fable.Declaration) : byte array =

        [||]

module Compiler =
    open Util
    //per file
    type WasmCompiler(com: Fable.Compiler) =

        interface IWasmCompiler with
            member wcom.GetIdentifier(ctx: Context, name: string) : string = getIdentifier wcom ctx name

        interface Compiler with
            member _.Options = com.Options
            member _.Plugins = com.Plugins
            member _.LibraryDir = com.LibraryDir
            member _.CurrentFile = com.CurrentFile
            member _.OutputDir = com.OutputDir
            member _.OutputType = com.OutputType
            member _.ProjectFile = com.ProjectFile
            member _.SourceFiles = com.SourceFiles
            member _.IncrementCounter() = com.IncrementCounter()
            member _.IsPrecompilingInlineFunction = com.IsPrecompilingInlineFunction
            member _.WillPrecompileInlineFunction(file) = com.WillPrecompileInlineFunction(file)
            member _.GetImplementationFile(fileName) = com.GetImplementationFile(fileName)
            member _.GetRootModule(fileName) = com.GetRootModule(fileName)
            member _.TryGetEntity(fullName) = com.TryGetEntity(fullName)
            member _.GetInlineExpr(fullName) = com.GetInlineExpr(fullName)
            member _.AddWatchDependency(fileName) = com.AddWatchDependency(fileName)

            member _.AddLog(msg, severity, ?range, ?fileName: string, ?tag: string) =
                com.AddLog(msg, severity, ?range = range, ?fileName = fileName, ?tag = tag)

    let makeCompiler com = WasmCompiler(com)

    let transformFile (com: Compiler) (file: Fable.File) =
        let wcom = makeCompiler com :> IWasmCompiler

        let ctx = { File = file }

        let wasmBytes = transformDeclaration wcom ctx file.Declarations.[0]

        wasmBytes
