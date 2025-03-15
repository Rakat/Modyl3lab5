using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class MainForm : Form
    {
        private List<string> photoPaths = new List<string>();
        private int currentPhotoIndex = 0;

        public MainForm()
        {
            InitializeComponent();
            InitializeCarOptions();
            InitializeFolderViewer();
        }

        // Завдання 1: Розрахунок вартості автомобіля
        private void InitializeCarOptions()
        {
            // Встановлюємо фото за замовчуванням
            pictureBoxCar.Image = Image.FromFile("car.jpg");

            // Ініціалізація CheckBox для вибору опцій
            checkBoxLeatherSeats.CheckedChanged += (sender, e) => CalculateCarPrice();
            checkBoxSunroof.CheckedChanged += (sender, e) => CalculateCarPrice();
            checkBoxNavigation.CheckedChanged += (sender, e) => CalculateCarPrice();
        }

        private void CalculateCarPrice()
        {
            double basePrice = 20000;
            double extraPrice = 0;

            if (checkBoxLeatherSeats.Checked) extraPrice += 2000;
            if (checkBoxSunroof.Checked) extraPrice += 1500;
            if (checkBoxNavigation.Checked) extraPrice += 1000;

            double totalPrice = basePrice + extraPrice;
            textBoxCarPrice.Text = $"Загальна вартість: {totalPrice} грн.";
        }

        // Завдання 2: Перегляд папок і файлів
        private void InitializeFolderViewer()
        {
            // Завантажуємо дерево папок
            treeViewFolders.AfterSelect += TreeViewFolders_AfterSelect;
            LoadFolders();
        }

        private void LoadFolders()
        {
            treeViewFolders.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Мої документи");
            treeViewFolders.Nodes.Add(rootNode);
            LoadSubDirectories("C:\\", rootNode);
        }

        private void LoadSubDirectories(string path, TreeNode node)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    TreeNode subDirNode = new TreeNode(Path.GetFileName(dir));
                    node.Nodes.Add(subDirNode);
                    LoadSubDirectories(dir, subDirNode);
                }
                foreach (var file in Directory.GetFiles(path))
                {
                    node.Nodes.Add(new TreeNode(Path.GetFileName(file)));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Якщо немає доступу до деяких папок
            }
        }

        private void TreeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listBoxFiles.Items.Clear();
            string selectedPath = Path.Combine("C:\\", e.Node.FullPath);
            if (Directory.Exists(selectedPath))
            {
                foreach (var file in Directory.GetFiles(selectedPath))
                {
                    listBoxFiles.Items.Add(Path.GetFileName(file));
                }
            }
        }

        // Завдання 3: Перегляд фотографій
        private void listBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = listBoxFiles.SelectedItem.ToString();
            string fullPath = Path.Combine("C:\\", selectedFile);

            if (File.Exists(fullPath) && (fullPath.EndsWith(".jpg") || fullPath.EndsWith(".png")))
            {
                pictureBoxPhoto.Image = Image.FromFile(fullPath);
            }
        }

        // Завдання 4: Слайд-шоу фотографій
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (photoPaths.Count > 0)
            {
                currentPhotoIndex = (currentPhotoIndex + 1) % photoPaths.Count;
                pictureBoxSlideShow.Image = Image.FromFile(photoPaths[currentPhotoIndex]);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (photoPaths.Count > 0)
            {
                currentPhotoIndex = (currentPhotoIndex - 1 + photoPaths.Count) % photoPaths.Count;
                pictureBoxSlideShow.Image = Image.FromFile(photoPaths[currentPhotoIndex]);
            }
        }

        private void btnAddPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    photoPaths.Add(dialog.FileName);
                    pictureBoxSlideShow.Image = Image.FromFile(photoPaths[0]);
                }
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            if (pictureBoxSlideShow.Image != null)
            {
                pictureBoxSlideShow.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBoxSlideShow.Refresh();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
