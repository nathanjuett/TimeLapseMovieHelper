using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

namespace ConsoleApplication1
{
    public static class Utilities
    {
        public static void CopyFilesIntoDirectories(string directory)
        {
            foreach (var item in System.IO.Directory.GetFiles(directory))
            {
                DateTime dt = System.IO.File.GetLastWriteTime(item).Date;
                string folder = dt.Month.ToString() + "-" + dt.Day.ToString();
                string fullpath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(item), folder);
                if (!System.IO.Directory.Exists(fullpath))
                {
                    System.IO.Directory.CreateDirectory(fullpath);
                Console.WriteLine("Directory Created : " + fullpath);
                }
                if (!System.IO.File.Exists(System.IO.Path.Combine(fullpath, System.IO.Path.GetFileName(item))))
                {
                    System.IO.File.Move(item, System.IO.Path.Combine(fullpath, System.IO.Path.GetFileName(item)));
                    Console.WriteLine("Moved : " + item);
                }
                else
                {

                    File.Delete(item);
                    Console.WriteLine("Deleteed : " + item);
                }
            }
        }
        public static void BuildMovieProjects(string directory, string projectDirectory)
        {
            foreach (var item in System.IO.Directory.GetDirectories(directory))
            {
                string ProjectFilename = "OldTexaco" + Path.GetFileName(item) + ".wlmp";
                string movieproject = Path.Combine(projectDirectory, ProjectFilename);
                if (File.Exists(movieproject))
                    continue;

                List<XElement> mediaitems = new List<XElement>();
                List<XElement> ImageClips = new List<XElement>();
                List<XElement> ExtentSelectors = new List<XElement>();
                List<XElement> ExtentRefs = new List<XElement>();

                int filecount = 1;
                foreach (var file in Directory.GetFiles(item))
                {
                    GetMediaItems(mediaitems, filecount, file);
                    GetImageClips(ImageClips, filecount);
                    GetExtentRefs(ExtentRefs, filecount);
                    filecount++;
                }
                BuildExtentSelectors(ExtentSelectors, ExtentRefs);
                CreateDoc(movieproject, ProjectFilename, mediaitems, ImageClips, ExtentSelectors);
                Console.WriteLine(ProjectFilename);
            }
        }

        private static void BuildExtentSelectors(List<XElement> ExtentSelectors, List<XElement> ExtentRefs)
        {
            XElement ExtentSelector1 = new XElement("ExtentSelector",
                                 new XAttribute("extentID", 1),
                                 new XAttribute("gapBefore", "0"),
                                 new XAttribute("primaryTrack", "true"),
                                 new XElement("Effects"),
                                 new XElement("Transitions"),
                                 new XElement("BoundProperties"),
                                 new XElement("ExtentRefs", ExtentRefs));
            ExtentSelectors.Add(ExtentSelector1);
            XElement ExtentSelector2 = new XElement("ExtentSelector",
                  new XAttribute("extentID", 2),
                  new XAttribute("gapBefore", "0"),
                  new XAttribute("primaryTrack", "false"),
                  new XElement("Effects"),
                  new XElement("Transitions"),
                  new XElement("BoundProperties"),
                  new XElement("ExtentRefs"));
            ExtentSelectors.Add(ExtentSelector2);
            XElement ExtentSelector3 = new XElement("ExtentSelector",
                  new XAttribute("extentID", 3),
                  new XAttribute("gapBefore", "0"),
                  new XAttribute("primaryTrack", "false"),
                  new XElement("Effects"),
                  new XElement("Transitions"),
                  new XElement("BoundProperties"),
                  new XElement("ExtentRefs"));
            ExtentSelectors.Add(ExtentSelector3);
            XElement ExtentSelector4 = new XElement("ExtentSelector",
                  new XAttribute("extentID", 4),
                  new XAttribute("gapBefore", "0"),
                  new XAttribute("primaryTrack", "false"),
                  new XElement("Effects"),
                  new XElement("Transitions"),
                  new XElement("BoundProperties"),
                  new XElement("ExtentRefs"));
            ExtentSelectors.Add(ExtentSelector4);
        }

        private static void GetExtentRefs(List<XElement> ExtentRefs, int filecount)
        {
            XElement ExtentRef = new XElement("ExtentRef",
                new XAttribute("id", filecount + 4));
            ExtentRefs.Add(ExtentRef);
        }

        private static void CreateDoc(string fullpath, string ProjectFilename, List<XElement> mediaitems, List<XElement> ImageClips, List<XElement> ExtentSelectors)
        {
            XDocument xdoc = new XDocument(
                new XDeclaration("1.0", "utf-8", ""),
                new XElement("Project",
                    new XAttribute("name", ProjectFilename.Replace(".wlmp", "")),
                    new XAttribute("themeId", "0"),
                    new XAttribute("version", "65540"),
                    new XAttribute("templateID", "SimpleProjectTemplate"),
                    new XElement("MediaItems", mediaitems),
                    new XElement("Extents", ImageClips, ExtentSelectors),
                    new XElement("BoundPlaceholders",
                        new XElement("BoundPlaceholder",
                            new XAttribute("placeholderID", "SingleExtentView"),
                            new XAttribute("extentID", "0")),
                        new XElement("BoundPlaceholder",
                            new XAttribute("placeholderID", "Main"),
                            new XAttribute("extentID", "1")),
                        new XElement("BoundPlaceholder",
                            new XAttribute("placeholderID", "SoundTrack"),
                            new XAttribute("extentID", "2")),
                        new XElement("BoundPlaceholder",
                            new XAttribute("placeholderID", "Narration"),
                            new XAttribute("extentID", "3")),
                        new XElement("BoundPlaceholder",
                            new XAttribute("placeholderID", "Text"),
                            new XAttribute("extentID", "4"))
                        ),
                    new XElement("BoundProperties",
                            new XElement("BoundPropertyFloatSet",
                                new XAttribute("Name", "AspectRatio"),
                                new XElement("BoundPropertyFloatElement",
                                   new XAttribute("Value", "1.7777776718139648"))),
                                new XElement("BoundPropertyFloat",
                                   new XAttribute("Name", "DuckedNarrationAndSoundTrackMix"),
                                   new XAttribute("Value", "0.5")),
                                new XElement("BoundPropertyFloat",
                                   new XAttribute("Name", "DuckedVideoAndNarrationMix"),
                                   new XAttribute("Value", "0.5")),
                                new XElement("BoundPropertyFloat",
                                   new XAttribute("Name", "DuckedVideoAndSoundTrackMix"),
                                   new XAttribute("Value", "0.5")),
                                new XElement("BoundPropertyFloat",
                                   new XAttribute("Name", "SoundTrackMix"),
                                   new XAttribute("Value", "0.5"))),
                    new XElement("ThemeOperationLog",
                        new XAttribute("themeID", "0"),
                        new XElement("MonolithicThemeOperations")),
                    new XElement("AudioDuckingProperties",
                        new XAttribute("emphasisPlaceholderID", "Narration"))
                    ));
            xdoc.Save(fullpath);
        }

        private static void GetImageClips(List<XElement> ImageClips, int filecount)
        {
            XElement imageClip = new XElement("ImageClip",
                new XAttribute("extentID", filecount + 4),
                new XAttribute("gapBefore", "0"),
                new XAttribute("mediaItemID", filecount),
                new XAttribute("duration", "0.029999999999999999"),
                new XElement("Effects"),
                new XElement("Transitions"),
                new XElement("BoundProperties",
                    new XElement("BoundPropertyInt",
                        new XAttribute("Name", "rotateStepNinety"),
                        new XAttribute("Value", "0"))));
            ImageClips.Add(imageClip);
        }

        private static void GetMediaItems(List<XElement> mediaitems, int filecount, string file)
        {
            int width;
            int height;
            using (Image i = Image.FromFile(file))
            {
                width = i.Size.Width;
                height = i.Size.Height;
            }
            XElement mediaitem = new XElement("MediaItem",
                new XAttribute("id", filecount),
                new XAttribute("filePath", file),
                new XAttribute("arWidth", width),
                new XAttribute("arHeight", height),
                new XAttribute("duration", "0"),
                new XAttribute("songTitle", ""),
                new XAttribute("songArtist", ""),
                new XAttribute("songAlbum", ""),
                new XAttribute("songCopyrightUrl", ""),
                new XAttribute("songArtistUrl", ""),
                new XAttribute("songAudioFileUrl", ""),
                new XAttribute("stabilizationMode", "0"),
                new XAttribute("mediaItemType", "2"));
            mediaitems.Add(mediaitem);
        }

    }
}
