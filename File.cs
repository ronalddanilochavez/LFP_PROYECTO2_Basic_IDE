using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFP_PROYECTO2_Basic_IDE
{
    class File
    {
        public string[] createFileProjectGTP(RichTextBox rtb)
        {
            // fileProject[0] is the project's file and fileProject[1] is the IDE's file
            string[] fileProject = new string[2];

            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                // To show some title
                saveFile.Title = "Guardar Archivo de IDE";
                // set a default file name
                saveFile.FileName = "";
                // set filters - this can be done in properties as well
                saveFile.Filter = "Archivos GT (*.gt)|*.gt|Todos los archivos (*.*)|*.*";
                // Directory to start
                saveFile.InitialDirectory = ".";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    //using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                    //sw.WriteLine("Hello World!");
                    rtb.SaveFile(saveFile.FileName);
                    fileProject[1] = saveFile.FileName;
                }

                SaveFileDialog saveFileProject = new SaveFileDialog();
                // To show some title
                saveFileProject.Title = "Crear Archivo de Proyecto";
                // set a default file name
                saveFileProject.FileName = "";
                // set filters - this can be done in properties as well
                saveFileProject.Filter = "Archivos GTP (*.gtP)|*.gtP|Todos los archivos (*.*)|*.*";
                // Directory to start
                saveFileProject.InitialDirectory = ".";

                if (saveFileProject.ShowDialog() == DialogResult.OK)
                {
                    rtb.Text = saveFile.FileName;
                    rtb.SaveFile(saveFileProject.FileName);
                    fileProject[0] = saveFileProject.FileName;
                    rtb.Clear();
                }

                // Loads the project and IDE files
                rtb.LoadFile(fileProject[0]);
                rtb.LoadFile(fileProject[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fileProject;
        }

        public string[] openFileProjectGTP(RichTextBox rtb)
        {
            // fileProject[0] is the project's file and fileProject[1] is the IDE's file
            string[] fileProject = new string[2];

            try
            {
                OpenFileDialog openFileProject = new OpenFileDialog();
                // To show some title
                openFileProject.Title = "Abrir Archivo de Proyecto";
                // set a default file name
                openFileProject.FileName = "";
                // set filters - this can be done in properties as well
                openFileProject.Filter = "Archivos GTP (*.gtP)|*.gtP|Todos los archivos (*.*)|*.*";
                // Directory to start
                openFileProject.InitialDirectory = ".";

                if (openFileProject.ShowDialog() == DialogResult.OK)
                {
                    rtb.LoadFile(openFileProject.FileName);
                    fileProject[0] = openFileProject.FileName;
                    rtb.LoadFile(rtb.Text);
                    fileProject[1] = rtb.Text;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fileProject;
        }

        public void saveFileProjectGTP(RichTextBox rtb, string fileProject, string fileIDE)
        {
            // fileProject[0] is the project's file and fileProject[1] is the IDE's file

            try
            {
                rtb.SaveFile(fileIDE);
                rtb.LoadFile(fileProject);
                rtb.SaveFile(fileProject);
                rtb.LoadFile(fileIDE);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void openFileIDEGT(RichTextBox rtb)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                // To show some title
                openFile.Title = "Abrir Archivo de IDE";
                // set a default file name
                openFile.FileName = "";
                // set filters - this can be done in properties as well
                openFile.Filter = "Archivos GT (*.gt)|*.gt|Todos los archivos (*.*)|*.*";
                // Directory to start
                openFile.InitialDirectory = ".";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    rtb.LoadFile(openFile.FileName);
                    //MessageBox.Show(openFile.FileName.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void saveFileIDEGT(RichTextBox rtb)
        {
            try
            {
                SaveFileDialog saveFileProject = new SaveFileDialog();
                // To show some title
                saveFileProject.Title = "Guardar Archivo de IDE";
                // set a default file name
                saveFileProject.FileName = "";
                // set filters - this can be done in properties as well
                saveFileProject.Filter = "Archivos GT (*.gt)|*.gt|Todos los archivos (*.*)|*.*";
                // Directory to start
                saveFileProject.InitialDirectory = ".";

                if (saveFileProject.ShowDialog() == DialogResult.OK)
                {
                    //using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                    //sw.WriteLine("Hello World!");
                    rtb.SaveFile(saveFileProject.FileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void openFileLogGTE(RichTextBox rtb)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                // To show some title
                openFile.Title = "Abrir Archivo de Log";
                // set a default file name
                openFile.FileName = "";
                // set filters - this can be done in properties as well
                openFile.Filter = "Archivos GTE (*.gtE)|*.gtE|Todos los archivos (*.*)|*.*";
                // Directory to start
                openFile.InitialDirectory = ".";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    rtb.LoadFile(openFile.FileName);
                    //MessageBox.Show(openFile.FileName.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void saveFileLogGTE(RichTextBox rtb)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                // To show some title
                saveFile.Title = "Guardar Archivo de Log";
                // set a default file name
                saveFile.FileName = "";
                // set filters - this can be done in properties as well
                saveFile.Filter = "Archivos GTE (*.gtE)|*.gtE|Todos los archivos (*.*)|*.*";
                // Directory to start
                saveFile.InitialDirectory = ".";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    //using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                    //sw.WriteLine("Hello World!");
                    rtb.SaveFile(saveFile.FileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
