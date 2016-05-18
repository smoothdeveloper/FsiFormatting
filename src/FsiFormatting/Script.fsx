#load "FsiFormatting.fs"
open FsiFormatting

module Printers =
  open Html
  open System.Text
  let dataTable (table: System.Data.DataTable) =
    let builder = StringBuilder()
    let append (text: string) = builder.Append text |> ignore
    let appendHtmlEncode (text: obj) = text |> string |> htmlEncode |> append

    append "<table>"

    // output header
    append "<thead>"
    append "<tr>"
    for column in table.Columns do
      append "<th>"
      appendHtmlEncode column.ColumnName
      append "</th>"
    append "</tr>"
    append "</thead>"

    // output body
    append "<tbody>"
    for row in table.Rows do
      append "<tr>"
      for cell in row.ItemArray do
        append "<td>"
        appendHtmlEncode cell
        append "</td>"

      append "</tr>"
    append "<tbody>"
    append "</tbody>"
    append "</table>"
    Result.makeHtml (string builder)

let table = new System.Data.DataTable()
table.Columns.Add("a") |> ignore
[1..100] |> Seq.iter(fun i -> table.Rows.Add(box i) |> ignore)

Printers.dataTable table
|> Result.getHtmlContents
|> Option.iter (printfn "%s")