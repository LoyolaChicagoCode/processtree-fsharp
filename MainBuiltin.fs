module ProcessTree.MainBuiltin

open System
open System.IO

let bOut = new StreamWriter(new BufferedStream(Console.OpenStandardOutput()))
let header = Console.ReadLine()
let ws = [| '\t'; ' ' |]
let cols = header.Split(ws, StringSplitOptions.RemoveEmptyEntries)
let iPid = Array.IndexOf(cols, "PID")
let iPpid = Array.IndexOf(cols, "PPID")
let iCmd = Math.Max(header.IndexOf("CMD"), header.IndexOf("COMMAND"))

let rec lines = seq {
    let line = Console.ReadLine()
    if line <> null then yield line ; yield! lines
}

let parse (line: string) =
    let words = line.Substring(0, iCmd).Split(ws, StringSplitOptions.RemoveEmptyEntries)
    Int32.Parse(words.[iPid]), Int32.Parse(words.[iPpid]), line.Substring(iCmd)

let ps = lines |> Seq.map parse |> List.ofSeq
let cMap = ps |> List.map (fun (pid, _, cmd) -> (pid, cmd)) |> Map.ofList
let tMap = ps |> Seq.ofList |> Seq.groupBy (fun(_, ppid, _) -> ppid) |> Map.ofSeq

let rec PrintTree l (i: int) =
    for j = 1 to l do
        bOut.Write(' ')
    bOut.Write(i)
    bOut.Write(':')
    bOut.Write(' ')
    bOut.Write(cMap.[i])
    bOut.Write('\n')
    if tMap.ContainsKey(i) then
        for k, _, _ in tMap.[i] do PrintTree (l + 1) k

for k, _, _ in tMap.[0] do PrintTree 0 k
bOut.Flush()
