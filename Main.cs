// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="">
//   Copyright (c) 2022 
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Mastercam.App;
using Mastercam.App.Exceptions;
using Mastercam.App.Types;
using Mastercam.Database;
using Mastercam.Database.Types;
using Mastercam.GeometryUtility;
using Mastercam.Interop.MachineGroup;
using Mastercam.IO;
using Mastercam.IO.Types;
using Mastercam.Math;
using Mastercam.Operations;
using Mastercam.Support;
using Mastercam.Support.UI;
using RoutechToFiveAxis.Properties;
using RoutechToFiveAxis.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RoutechToFiveAxis
{
    /// <summary> Describes the main class. </summary>
    public class Main : NetHook3App
    {
        // A special thanks for the convert Icon by Mohit Gandhi https://iconscout.com/contributors/mcgandhi61

        #region Machine & Tool Files
        public const string fiveAxisToolOps = "G:\\CAD Support\\CNC\\FiveAxisToolpaths.mcam";
        public const string fiveAxisMachine = "S:\\MasterCAM Posts\\MC-2020\\CNC MACHINES\\MPPOSTABILITY_XILOG_PLUS 5AXIS.mcam-rmd";
        public const string fiveAxisToolDb = "S:\\MasterCAM Posts\\MC-2020\\TOOL LIBRARIES\\5 AXIS 2018.tooldb";

        /*
         * If you wish to debug, you need to have your tool operations/part/machine
         * definitions on your C drive. no way around that as of Mastercam 2020
         */

        //For debugging inside VS. Be sure to modify these to match your local folder.
        public const string fiveAxisToolOpsT = "C:\\Users\\zstockton\\Documents\\Nhooks\\Nhook testing\\FiveAxisToolpaths.mcam";
        public const string fiveAxisMachineT = "C:\\Users\\zstockton\\Documents\\Nhooks\\Nhook testing\\CNC MACHINES\\MPPOSTABILITY_XILOG_PLUS 5AXIS.mcam-rmd";
        public const string fiveAxisToolDbT = "C:\\Users\\zstockton\\Documents\\Nhooks\\Nhook testing\\TOOL LIBRARIES\\5 AXIS 2018.tooldb";

        public readonly string[] contourToolDiameters = { ".125", ".25", ".375", ".5", ".75", "20MM", "V-FOLD" };
        public readonly string[] drillToolDiameters = { "8MM", "5MM", "3MM" };
        #endregion

        #region Public Override Methods

        /// <summary> Initialize anything we need for the NET-Hook here. </summary>
        ///
        /// <param name="param"> System parameter. </param>
        ///
        /// <returns> A <c>MCamReturn</c> return type representing the outcome of your NetHook application. </returns>
        public override MCamReturn Init(int param)
        {
            // Wire up handler for any global exceptions not handled by the app
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                this.HandleUnhandledException(args.ExceptionObject as Exception);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) => this.HandleUnhandledException(args.Exception);

            if (Settings.Default.FirstTimeRunning)
            {
                var msg = ResourceReaderService.GetString("FirstTimeRunning");
                var assembly = Assembly.GetExecutingAssembly().FullName;
                EventManager.LogEvent(MessageSeverityType.InformationalMessage, assembly, msg.IsSuccess ? msg.Value : msg.Error);

                Settings.Default.FirstTimeRunning = false;
                Settings.Default.Save();
            }

            return base.Init(param);
        }

        /// <summary> The main entry point for your RoutechToFiveAxis. </summary>
        ///
        /// <param name="param"> System parameter. </param>
        ///
        /// <returns> A <c>MCamReturn</c> return type representing the outcome of your NetHook application. </returns>
        public override MCamReturn Run(int param)
        {
            // Create our view
            var winView = new MainView { TopLevel = true };

            // Set the dialog as modeless to Mastercam, always on top
            var handle = Control.FromHandle(MastercamWindow.GetHandle().Handle);
            _ = new ModelessDialogTabsHandler(winView);

            winView.StartPosition = FormStartPosition.CenterScreen;
            winView.Show(handle);

            return MCamReturn.NoErrors;
        }
        #endregion

        #region Public User Defined Methods

        /// <summary> The custom user function entry point for your RoutechToFiveAxis. </summary>
        ///
        /// <param name="param"> System parameter. </param>
        ///
        /// <returns> A <c>MCamReturn</c> return type representing the outcome of your NetHook application. </returns>
        public MCamReturn RunUserDefined(int param)
        {
            // read project resource strings
            var userMessage = ResourceReaderService.GetString("UserMessage");
            var title = ResourceReaderService.GetString("Title");

            DialogManager.OK(
                userMessage.IsSuccess ? userMessage.Value : userMessage.Error,
                title.IsSuccess ? title.Value : title.Error);
            return MCamReturn.NoErrors;
        }

        public MCamReturn RunToLarry()
        {
            ChangeMachineAndToolDb();

            MoveGeometryAndRegenStock();

            CheckAndChangeContourSettings("LARRY");

            CheckDrillSettings("LARRY");

            if (Cleanup() == false)
            {
                DialogManager.Error("Clean-Up had a problem! This likely has to do with the naming conventions of the containing and destination folder(s)", "Clean-Up Error");
            }
            else
            {
                DialogManager.OK("Clean-Up done! DO NOT save the file if you want to keep the old program!!", "NOTICE: Clean-Up Done");
            }

            return MCamReturn.NoErrors;
        }

        public MCamReturn RunToMike()
        {
            ChangeMachineAndToolDb();

            MoveGeometryAndRegenStock();

            CheckAndChangeContourSettings("MIKE");

            CheckDrillSettings("MIKE");

            if (Cleanup() == false)
            {
                DialogManager.Error("Clean-Up had a problem! This likely has to do with the naming conventions of the containing and destination folder(s)", "Clean-Up Error");
            }
            else
            {
                DialogManager.OK("Clean-Up done! DO NOT save the file if you want to keep the old program!!", "NOTICE: Clean-Up Done");
            }

            return MCamReturn.NoErrors;
        }

        public MCamReturn RunToGrant()
        {
            ChangeMachineAndToolDb();

            MoveGeometryAndRegenStock();

            CheckAndChangeContourSettings("GRANT");

            CheckDrillSettings("GRANT");

            if (Cleanup() == false)
            {
                DialogManager.Error("Clean-Up had a problem! This likely has to do with the naming conventions of the containing and destination folder(s)", "Clean-Up Error");
            }
            else
            {
                DialogManager.OK("Clean-Up done! DO NOT save the file if you want to keep the old program!!", "NOTICE: Clean-Up Done");
            }

            return MCamReturn.NoErrors;
        }
        #endregion

        #region Private Methods

        /// <summary> Log exceptions and show a message. </summary>
        ///
        /// <param name="e"> The exception. </param>
        private void HandleUnhandledException(Exception e)
        {
            // Show the user
            DialogManager.Exception(new MastercamException(e.Message, e.InnerException));

            // Write to the event log
            var msg = e.InnerException != null ? e.InnerException.Message : e.Message;
            var assembly = Assembly.GetExecutingAssembly().FullName;
            EventManager.LogEvent(MessageSeverityType.ErrorMessage, assembly, msg);
        }

        /// <summary>
        /// Replaces the current tool database and machine definition to the Morbidelli 5-axis
        /// </summary>
        private void ChangeMachineAndToolDb()
        {
            //Change machine to 5-axis
            GroupManager.SetMachineInActiveGroup(fiveAxisMachine);
            OperationsManager.ToolLibraryName = fiveAxisToolDb;

        }

        /// <summary>
        /// Translates all part geometry and regenerates the stock.
        /// </summary>
        /// <returns>True if the translation was successful. False if otherwise.</returns>
        private bool MoveGeometryAndRegenStock()
        {
            MillStockData data = MillStock.GetStockData();

            //take existing stock and put it into a variable, also calculate the offset of the part if there is any

            Point3D topOfPart = new Point3D(data.StockOrigin.x * 2, data.StockOrigin.y * 2, 0);
            Point3D stockSize = OperationsManager.JobSetupStockSize;
            double offset = (topOfPart.y - stockSize.y) / 2;
            Point3D newPosition = new Point3D(topOfPart.x, -topOfPart.y + offset, stockSize.z);

            //Select all geometry and perform a translate down into the 4th quadrant
            SelectionManager.SelectAllGeometry();

            bool translateSucceeded = GeometryManipulationManager.TranslateGeometry(new Point3D(0, 0, 0), new Point3D(0, (newPosition.y), 0), SearchManager.GetSystemView(SystemPlaneType.Top), SearchManager.GetSystemView(SystemPlaneType.Top), false);
            if (!translateSucceeded)
            {
                DialogManager.Error("Geometry did not translate.", "XForm-Translate error");
                return false;
            }
            GraphicsManager.Repaint(true);

            //Reassign stock origin and settings
            data.StockSize = new Point3D(stockSize.x, stockSize.y + offset, stockSize.z);
            data.StockOrigin = new Point3D(newPosition.x * 0.5, (-data.StockSize.y) * 0.5, 0);
            data.DisplayStock = true;
            data.SolidStock = true;
            MillStock.SetStockData(data);

            if (offset > 0)
            {
                DialogManager.OK($"Part has an offset of {offset}\". Bottom of the stock has been extended to match the offset", "Part Offset Detected");
            }

            return translateSucceeded;
        }

        /// <summary>
        /// Checks every contour and pocket operation to modify the operation settings based on the operator's machine.
        /// </summary>
        /// <param name="whichMachine">The machine the part is going to. Referenced by the operator's name</param>
        private void CheckAndChangeContourSettings(string whichMachine)
        {
            //Get all contour operations in the file and loop through each one to adjust the contour/pocket operations parameters
            
            //ContourSubtype has a bug where it always returns basic, so use LINQ to search outside of the loop.
            var rampContours = SearchManager.GetOperations(OperationType.Contour)
                    .OfType<ContourOperation>()
                    .Where(r => r.CutParams.ContourSubtype == Mastercam.Operations.Types.ContourSubtypeType.Ramp2D)
                    .ToList();
            var pocketContours = SearchManager.GetOperations(OperationType.Pocket)
                .OfType<PocketOperation>()
                .ToList();
            var normalContours = SearchManager.GetOperations(OperationType.Contour)
                .OfType<ContourOperation>()
                .Where(n => n.CutParams.ContourSubtype == Mastercam.Operations.Types.ContourSubtypeType.Basic)
                .ToList();

            if (pocketContours.Any())
            {
                foreach (Operation op in pocketContours)
                {
                    PocketOperation pocketOp = op as PocketOperation;

                    CheckAndChangeTool(contourToolDiameters, pocketOp, whichMachine);

                    pocketOp.Commit();
                }
            }

            if (normalContours.Any())
            {
                foreach (Operation op in normalContours)
                {
                    //Based on the operation type, modify the settings and change the tools
                    ContourOperation contourOp = op as ContourOperation;

                    contourOp.LeadInOut.Exit.Enabled = true;

                    //Larry's machine is.. special. So turn off plunge/retract and turn on computer comp.
                    if (whichMachine == "LARRY")
                    {
                        //Compensation is usually off for a reason, so if it is, leave it like that
                        if (contourOp.CutterComp.Type != CutterCompType.CutterCompOff)
                        {
                            contourOp.CutterComp.Type = CutterCompType.CutterCompComputer;
                        }
                        contourOp.LeadInOut.Entry.PlungeAfterFirstOrRetractBeforeLastMove = false;
                    }
                    else
                    {
                        //These settings do the opposite of Larry's for the other operators
                        contourOp.LeadInOut.Entry.PlungeAfterFirstOrRetractBeforeLastMove = true;
                        if (contourOp.CutterComp.Type != CutterCompType.CutterCompOff)
                        {
                            contourOp.CutterComp.Type = CutterCompType.CutterCompControl;
                        }
                    }

                    contourOp.LeadInOut.Exit = contourOp.LeadInOut.Entry;

                    CheckAndChangeTool(contourToolDiameters, contourOp, whichMachine);

                    contourOp.Commit();
                }
            }

            if (rampContours.Any())
            {
                foreach (Operation op in rampContours)
                {
                    ContourOperation contourOp = op as ContourOperation;

                    //Larry's machine is.. special. So turn off plunge/retract and turn on computer comp.
                    if (whichMachine == "LARRY")
                    {
                        //Compensation is usually off for a reason, so if it is, leave it like that
                        if (contourOp.CutterComp.Type != CutterCompType.CutterCompOff)
                        {
                            contourOp.CutterComp.Type = CutterCompType.CutterCompComputer;
                        }

                        contourOp.LeadInOut.Entry.ArcRadius = contourOp.OperationTool.Diameter * 0.3;
                        contourOp.LeadInOut.Entry.HelixHeight = 2;

                        contourOp.LeadInOut.Entry.PlungeAfterFirstOrRetractBeforeLastMove = false;
                    }
                    else
                    {
                        //These settings do the opposite of Larry's for the other operators
                        contourOp.LeadInOut.Entry.PlungeAfterFirstOrRetractBeforeLastMove = true;
                        if (contourOp.CutterComp.Type != CutterCompType.CutterCompOff)
                        {
                            contourOp.CutterComp.Type = CutterCompType.CutterCompControl;
                        }
                    }

                    contourOp.LeadInOut.Exit = contourOp.LeadInOut.Entry;

                    CheckAndChangeTool(contourToolDiameters, contourOp, whichMachine);

                    contourOp.Commit();
                }
            }
        }

        /// <summary>
        /// Checks the drill operations, and assigns tools based on the tool name with the 5-axis tool library
        /// </summary>
        /// <param name="whichMachine">The machine the part is going to. Referenced by the operator's name</param>
        private void CheckDrillSettings(string whichMachine)
        {
            var drillOps = SearchManager.GetOperations(OperationType.Drill)
                .OfType<DrillOperation>()
                .ToList();

            var blockOps = SearchManager.GetOperations(OperationType.BlockDrill)
                .OfType<BlockDrillOperation>()
                .ToList();

            //Based on the operation type, change the tools
            if (drillOps.Any())
            {
                foreach (DrillOperation op in drillOps)
                {
                    CheckAndChangeTool(drillToolDiameters, op, whichMachine);
                }
            }

            if (blockOps.Any())
            {
                foreach (BlockDrillOperation op in blockOps)
                {
                    SwitchBlockDrill(op, whichMachine);
                }
            }
        }


        /// <summary>
        /// Compares the name of the given tool and changes it to the respective tool with the same name in the 5-axis tool library
        /// </summary>
        /// <param name="toolList">The string array that contains all of the replacable tools</param>
        /// <param name="selectedOperation">The operation that is having the tool changed</param>
        /// <param name="whichMachine">The machine the part is going to. Referenced by the operator's name</param>
        private void CheckAndChangeTool(string[] toolList, Operation selectedOperation, string whichMachine)
        {

            if (toolList.Equals(contourToolDiameters)) 
            {
                for (int i = 0; i < toolList.Length; i++)
                {
                    if (selectedOperation.OperationTool.Name.ToUpper().IndexOf($"{toolList[i]}") > -1)
                    {
                        //Contour/pocket checks
                        if (selectedOperation.OperationTool.Name.IndexOf("V-FOLD") > -1)
                        {
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                OperationName = $"{toolList[i]}",
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            var importedOp = OperationsManager.ImportOperation(importSettings);

                            selectedOperation.OperationTool = importedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();
                            importedOp.Delete();
                            return;
                        }
                        else if (selectedOperation.OperationTool.Name.IndexOf("RH") > -1)
                        {
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                OperationName = $"{toolList[i]} RH",
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            var importedOp = OperationsManager.ImportOperation(importSettings);

                            selectedOperation.OperationTool = importedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();
                            importedOp.Delete();
                            return;
                        }
                        else
                        {
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                OperationName = $"{toolList[i]} LH",
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            var leftHandImportedOp = OperationsManager.ImportOperation(importSettings);

                            selectedOperation.OperationTool = leftHandImportedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();

                            leftHandImportedOp.Delete();
                        }
                    }
                }
            }
            else
            {
                //Drill checks
                for (int i = 0; i < toolList.Length; i++)
                {
                    if (selectedOperation.OperationTool.Name.ToUpper().IndexOf($"{toolList[i]}") > -1)
                    {
                        if (selectedOperation.OperationTool.Name.ToUpper().IndexOf("8MM") > -1)
                        {
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                OperationName = $"{toolList[i]} ALL",
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            var importedOp = OperationsManager.ImportOperation(importSettings);

                            selectedOperation.OperationTool = importedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();
                            importedOp.Delete();
                            return;
                        }
                        else if (selectedOperation.OperationTool.Name.ToUpper().IndexOf("5MM") > -1)
                        {
                            Operation importedOp;
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            if (whichMachine == "LARRY" || whichMachine == "MIKE")
                            {
                                importSettings.OperationName = $"{toolList[i]} LARRY MIKE";
                                importedOp = OperationsManager.ImportOperation(importSettings);
                            }
                            else
                            {
                                importSettings.OperationName = $"{toolList[i]} GRANT";
                                importedOp = OperationsManager.ImportOperation(importSettings);
                            }

                            selectedOperation.OperationTool = importedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();
                            importedOp.Delete();
                            return;
                        }
                        else if (selectedOperation.OperationTool.Name.ToUpper().IndexOf("3MM") > -1)
                        {
                            Operation importedOp;
                            var importSettings = new OperationsManager.ImportOptions
                            {
                                FilePath = fiveAxisToolOps,
                                CaseSensitiveNameMatch = false,
                                DisableDuplicateToolCheck = true
                            };

                            if (whichMachine == "MIKE" || whichMachine == "GRANT")
                            {
                                importSettings.OperationName = $"{toolList[i]} MIKE GRANT";
                                importedOp = OperationsManager.ImportOperation(importSettings);
                            }
                            else
                            {
                                importSettings.OperationName = $"{toolList[i]} LARRY";
                                importedOp = OperationsManager.ImportOperation(importSettings);
                            }

                            selectedOperation.OperationTool = importedOp.OperationTool;

                            selectedOperation.OperationTool.Commit();
                            selectedOperation.Commit();
                            importedOp.Delete();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Trades the current operation's block drill with the 5-axis block drill
        /// </summary>
        /// <param name="blockOp">The operation that is being changed</param>
        /// <param name="whichMachine">The machine the part is going to. Referenced by the operator's name</param>
        private void SwitchBlockDrill(BlockDrillOperation blockOp, string whichMachine)
        {
            var importSettings = new OperationsManager.ImportOptions
            {
                FilePath = fiveAxisToolOps,
                CaseSensitiveNameMatch = false,
                DisableDuplicateToolCheck = true
            };

            if (whichMachine == "GRANT")
            {
                importSettings.OperationName = "BLOCK GRANT";
                var importedOp = OperationsManager.ImportOperation(importSettings);
                
                blockOp.OperationTool = importedOp.OperationTool;

                blockOp.OperationTool.Commit();
                blockOp.Commit();
                importedOp.Delete();
            }
            else if (whichMachine == "MIKE")
            {
                importSettings.OperationName = "BLOCK MIKE";
                var importedOp = OperationsManager.ImportOperation(importSettings);
                
                blockOp.OperationTool = importedOp.OperationTool;

                blockOp.OperationTool.Commit();
                blockOp.Commit();
                importedOp.Delete();
            }
            else
            {
                DialogManager.Error("Specified machine not defined for a block drill!", "Switch Block Drill Error");
            }
        }
        
        /// <summary>
        /// Refreshes the operations tree to remove deleted operations, as well as prompts for a save folder
        /// </summary>
        /// <returns>True if succeeded. False if not.</returns>
        private bool Cleanup()
        {
            //Force refresh on operations tree to remove deleted operations, then prompt the user to save 
            OperationsManager.RefreshOperationsManager(true);
            bool succeeded = true;
            string[] separator = { "ROUTECH", "02ROUTECH" };
            string sname = FileManager.CurrentFileName;
            string[] filePrefixes = sname.ToUpper().Split(separator, StringSplitOptions.RemoveEmptyEntries);

            //Ask if files should automatically save to Shoda folder
            switch (DialogManager.YesNoCancel("Would you like to save this file to the 5-axis folder? (Make sure there is a 5-Axis folder (03 5-Axis) in the directory!)", "Save File to 5-AXIS Folder?"))
            {
                case DialogReturnType.Yes:
                    succeeded = FileManager.SaveAs($"{filePrefixes[0]}03 5-AXIS{filePrefixes[1].ToLower()}");
                    break;
                case DialogReturnType.No:
                    FileManager.SaveAs();
                    break;
                case DialogReturnType.Cancel:
                    break;
                default:
                    succeeded = false;
                    break;
            }
            return succeeded;
        }

        #endregion
    }
}
