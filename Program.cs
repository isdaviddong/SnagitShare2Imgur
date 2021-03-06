﻿using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace SnagitShare2Imgur
{
    class Program
    {
        public class Options
        {
            [Option('i', "ImgurClientID", Required = false, HelpText = "Imgur App ClientID")]
            public string ImgurClientID { get; set; }


            [Option('f', "File", Required = false, HelpText = "File Path to Upload")]
            public string FilePath { get; set; }


            [Option('o', "Output", Required = false, HelpText = "Output Type")]
            public string outputType { get; set; }

            //public string GetUsage()
            //{
            //    return CommandLine.Text.HelpText.AutoBuild(this,
            //      (CommandLine.Text.HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            //}
        }

        static void RunOptions(Options o)
        {
            //there is no any argument
            if (_args.Length <= 0)
            {
                //show help
                showHelp();
                return;
            }

            //if only one parameter and suppose it's filename which 
            if (_args.Length == 1)
            {
                //get filename
                var filename = _args[0].Trim().ToLower();
                //get ext name
                var ext = System.IO.Path.GetExtension(filename).ToLower();
                if (ext == ".png" || ext == ".jpg" || ext == ".jepg" || ext == ".gif" || ext == ".mp4")
                {
                    //if valid format
                    upload(_args[0].Trim().ToLower(), OutputType.url);
                    return;
                }
            }

            //get first argument
            switch (_args[0].Trim().ToLower())
            {
                case "config":
                    config(_args, o);
                    break;
                case "upload":
                    upload(_args, o);
                    break;
                case "status":
                    status(_args, o);
                    break;
                default:
                    showHelp();
                    break;
            }
            return;
        }

        static private void upload(string FileName2Upload, OutputType outputType)
        {
            var JSON = "";
            try
            {
                var ImgurClientID = Utility.getImgurClientID();
                if (string.IsNullOrEmpty(ImgurClientID))
                {
                    Console.WriteLine("Missing ImgurClientID.");
                    return;
                }
                // file exist?
                if (!System.IO.File.Exists(FileName2Upload))
                {
                    Console.WriteLine($"file {FileName2Upload} is not exist.");
                    return;
                }
                // check file type
                var ext = System.IO.Path.GetExtension(FileName2Upload).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jepg" && ext != ".gif" && ext != ".mp4")
                {
                    Console.WriteLine($"file type {ext} is invalid.");
                    return;
                }
                Console.WriteLine($"reading file '{FileName2Upload}' ...");

                var FileBody = System.IO.File.ReadAllBytes(FileName2Upload);
                Console.WriteLine($"uploading ...");
                dynamic ret;
                if (ext != ".mp4")
                    ret = Utility.UploadImage2Imgur(ImgurClientID, FileBody);
                else
                    ret = Utility.UploadMp42Imgur(ImgurClientID, FileBody);
                JSON = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                Console.WriteLine($"uploaded. JSON:{JSON}");
                if (outputType == OutputType.url)
                    TextCopy.ClipboardService.SetText((string)ret.data.link);
                if (outputType == OutputType.markdown)
                    TextCopy.ClipboardService.SetText($"![]({(string)ret.data.link})");
                if (outputType == OutputType.html)
                    TextCopy.ClipboardService.SetText($"<img src='{(string)ret.data.link}' />");
                Console.WriteLine($"status: file {FileName2Upload} has been uploaded to {ret.data.link}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                Console.WriteLine($"Press any key to continue...");
                Console.ReadKey();
                throw ex;
            }

        }

        static private void upload(string[] args, Options o)
        {
            upload(o.FilePath, Enum.Parse<OutputType>(o.outputType));
        }

        //set up 
        static private void config(string[] args, Options o)
        {
            //if has ImgurClientID
            if (!string.IsNullOrEmpty(o.ImgurClientID))
            {
                //save to IsolatedStorage
                var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
                var file = store.OpenFile("ImgurClientID", FileMode.OpenOrCreate);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(o.ImgurClientID);
                    Console.WriteLine($"\n\nImgur ClientID has been updated : {o.ImgurClientID}\n");
                    return;
                }
            }
            else
            {
                showHelp();
            }
        }

        //show status
        static private void status(string[] args, Options o)
        {
            var rootDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Console.WriteLine($"Application Running Path : {rootDir}");
            Console.WriteLine($"Imgur ClientID : {Utility.getImgurClientID()}");
        }

        static void showHelp()
        {
            showHelp(_ParserResult, null);
        }

        static void showHelp<T>(ParserResult<T> parserResult, IEnumerable<Error> errs)
        {
            var helpText = CommandLine.Text.HelpText.AutoBuild(parserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                //h.Heading = "Myapp 2.0.0-beta"; //change header
                //h.Copyright = "Copyright (c) 2019 Global.com"; //change copyright text
                return CommandLine.Text.HelpText.DefaultParsingErrorsHandler(parserResult, h);
            }, e => e);
            Console.WriteLine(helpText);

            Console.WriteLine("Examples:");
            Console.WriteLine("SnagitShare2Imgur config -i {Imgur App ClientID}");
            Console.WriteLine("SnagitShare2Imgur upload -f {FilePath To Upload}");
        }

        static ParserResult<Options> _ParserResult = null;
        static string[] _args;
        static void Main(string[] args)
        {
            try
            {
                _args = args;
                _ParserResult = Parser.Default.ParseArguments<Options>(args);
                _ParserResult.WithParsed(RunOptions);
                //.WithNotParsed(HandleParseError);
                //.WithNotParsed(errs => showHelp(_ParserResult, errs));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message }\n");
                showHelp();
            }

            return;
        }

    }
}
