module main

open System
open System.IO
open System.Collections.Generic

let cMap = new Dictionary<int, String>()
let tMap = new Dictionary<int, ICollection<int>>()
let bOut = new StreamWriter(new BufferedStream(Console.OpenStandardOutput()))

let header = Console.ReadLine()
let ws = [| '\t'; ' ' |]
let cols = header.Split(ws, StringSplitOptions.RemoveEmptyEntries)
let iPid = Array.IndexOf(cols, "PID")
let iPpid = Array.IndexOf(cols, "PPID")
let iCmd = Math.Max(header.IndexOf("CMD"), header.IndexOf("COMMAND"))

let rec PrintTree l (i: int) =
    for j = 1 to l do
        bOut.Write(' ')
    bOut.Write(i)
    bOut.Write(':')
    bOut.Write(' ')
    bOut.Write(cMap.[i])
    bOut.Write('\n')
    if tMap.ContainsKey(i) then
        for k in tMap.[i] do PrintTree (l + 1) k

let mutable line = Console.ReadLine()
while (line <> null) do
    let words = line.Substring(0, iCmd).Split(ws, StringSplitOptions.RemoveEmptyEntries)
    let pid = Int32.Parse(words.[iPid])
    let ppid = Int32.Parse(words.[iPpid])
    cMap.[pid] <- line.Substring(iCmd)
    if not (tMap.ContainsKey(ppid)) then
        tMap.[ppid] <- new List<int>(8)
    tMap.[ppid].Add(pid)
    line <- Console.ReadLine()    

for k in tMap.[0] do PrintTree 0 k
bOut.Flush()
