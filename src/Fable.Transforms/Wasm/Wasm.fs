module rec Fable.AST.Wasm

type Literal =
    | IntLiteral of int32
    | LongLiteral of int64

type BinOp = | Plus
