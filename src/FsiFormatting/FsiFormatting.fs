namespace FsiFormatting
open System

type internal ImageOutput =
| Raw of byte array
| External of Uri

type internal SvgOutput = 
| Raw of string
| External of Uri

type internal HtmlOutput = { Data: string }
type internal JsonOutput = { Data: obj }

type internal Output =
| JSON of JsonOutput
| Html of HtmlOutput
| Svg of SvgOutput
| Binary of byte array
| Opaque of obj

type Result =
  private {
    ContentType: string
    Contents:    Output
  }

module Html =
  open System.Web
  let internal htmlEncode(str) = HttpUtility.HtmlEncode(str)

[<RequireQualifiedAccess; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Result =
  let makeHtml contents      = { ContentType = "text/html"; Contents = Html { HtmlOutput.Data = contents } }
  let getHtmlContents result =
    match result.Contents with
    | Html output -> Some output.Data
    | _ -> None

type 't formatter = 't -> Result
