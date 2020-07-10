using System;
using System.IO;
using System.Windows.Forms;

// LojRipper source code!
// Technically this can work in Mono. (it *kinda* supports WinForms)
// if you have problems with it, please contact me on discord nik#5351

namespace LojRipper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ButtonOpenFile_Click(object sender, EventArgs e)
        {
            FreeGameFromYYCShit();
        }

        private void FreeGameFromYYCShit()
        {
            if (CheckForPussy()) // just in case.
            {
                MessageBox.Show("go away. let people mod AM2R. please.", "please!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // exit safely.
            }
            else if (OpenExeDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = OpenExeDialog.OpenFile();
                if (stream == null)
                {
                    MessageBox.Show("Cannot open .exe, maybe it's locked by something? (perhaps, you're playing AM2R right now?)", "Uh oh.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BinaryReader reader = new BinaryReader(stream);
                    while (!CheckFORMHeader(reader)) { /* take ooooooon meeeeeeeee, taaaaaaaaaakeeeeee meeeeeeee oooooooooooooooooon TAKE ME OOOOOON */ }
                    uint size = reader.ReadUInt32();
                    reader.BaseStream.Seek(-8L, SeekOrigin.Current); // seek behind size and behind FORM (basically to the start)
                    size += 8U;
                    byte[] win_data = reader.ReadBytes((int)size); // read the whole .win (may be slow...)
                    reader.Dispose(); // dispose reader and the stream.

                    string path = Path.GetDirectoryName(OpenExeDialog.FileName) + Path.DirectorySeparatorChar + "yyc_dump.win";
                    try
                    {
                        File.Delete(path);
                        File.WriteAllBytes(path, win_data);
                        MessageBox.Show("Dumped OK! Now open that .win in UndertaleModTool and mod the game. Since you have the right to do it. (file saved as yyc_dump.win in the exe directory)", "Nice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Uh oh. Something's wrong. Exception:" + Environment.NewLine + exp.Message, "Uh oh.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        win_data = null; // free the array since it's obvious we can't write it.
                        // don't exit here since we want let user to try again.
                    }
                }
            }
        }

        private bool CheckForPussy()
        {
            // This function ensures that AM2R elitists won't be able to stop this tool... hahahahhaa lol jk they can read this anyway.

            return
                   // Lojemiru
                   Environment.UserName == "Lojical"  || // nick
                   Environment.UserName == "Lojemiru" || // nick 2
                   Environment.UserName == "Idiot who's scared of nintendo" || // hehe
                   Environment.UserName == "Matt"     || // check for "Matt" was suggested by [unknown], if you're Matt and you're not Lojemiru, sorry! (real name??)

                   // Metroid3D
                   Environment.UserName == "Metroid3D" || // nick
                   Environment.UserName == "Alex"      || // real name

                   // DruidVorse
                   Environment.UserName == "DruidVorse" || // nick
                   Environment.UserName == "Esteban"    || // real name

                   // sabre230
                   Environment.UserName == "sabre230" || // nick
                   Environment.UserName == "Steve"     ; // real name???
        }

        private bool CheckFORMHeader(BinaryReader reader)
        {
            //                                  "F"                          "O"                          "R"                          "M"
            bool isFORM = reader.ReadByte() == 0x46 && reader.ReadByte() == 0x4F && reader.ReadByte() == 0x52 && reader.ReadByte() == 0x4D;
            bool isGEN8 = false;
            if (isFORM)
            {
                reader.ReadUInt32(); // skip supposed length.
                //                             "G"                          "E"                          "N"                          "8"
                isGEN8 = reader.ReadByte() == 0x47 && reader.ReadByte() == 0x45 && reader.ReadByte() == 0x4E && reader.ReadByte() == 0x38;
                if (isGEN8)
                {
                    reader.BaseStream.Seek(-8L, SeekOrigin.Current); // seek behind GEN8 and behind FORM length. (we want to be right behind "FORM")
                }
            }

            return isFORM && isGEN8;
        }
    }
}
