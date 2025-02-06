module Fable.Transforms.Wasm.WasmPrinter

open System.IO
//open Fable.Transforms.Printer

//module Output =
//    let writeFile ctx (file: File) = ""
let isEmpty (bytes: byte array) : bool = false

let run (writer: Fable.Transforms.Printer.Writer) (bytes: byte array) : Async<unit> =
    async {
        //use printerImpl = new PrinterImpl(writer)
        //writer.
        do! writer.Write("")
    }
