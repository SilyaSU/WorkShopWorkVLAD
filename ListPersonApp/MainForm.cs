using WorkShopControlLib;

namespace WorkShop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            AddButton.BackColor = Color.FromArgb(73, 140, 81);
            PanelUsers.BackColor = Color.FromArgb(118, 227, 131);
        }

        private void Add_button(object sender, EventArgs e)
        {
            RecordsUserControl userControl = new RecordsUserControl();
            userControl.BackColor = Color.FromArgb(118, 227, 131);
            userControl.Click += UserControl_Click;
            userControl.StatusChanged += UserControl_StatusChanged;
            userControl.Date = DateTime.Now.ToString();
            PanelUsers.Controls.Add(userControl);
            UpdateCompletedCount();
        }

        private void UserControl_Click(object? sender, EventArgs e)
        {
            var userControl = (RecordsUserControl)((Control)sender).Parent;
            if (((Button)sender).Name == "buttonEdit")
            {
                RecordsEditDialog editDialog = new RecordsEditDialog(userControl);
                editDialog.IDDialog = userControl.ID;
                editDialog.DeviceDialog = userControl.Device;
                editDialog.DescriptionDialog = userControl.Description;
                editDialog.ErrorDialog = userControl.ErrorControl;
                editDialog.ClientDialog = userControl.Client;
                editDialog.EmploymentDialog = userControl.Employment;


                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    userControl.ID = editDialog.IDDialog;
                    userControl.Device = editDialog.DeviceDialog;
                    userControl.Description = editDialog.DescriptionDialog;
                    userControl.ErrorControl = editDialog.ErrorDialog;
                    userControl.Client = editDialog.ClientDialog;
                    userControl.Employment = editDialog.EmploymentDialog;
                }
                else
                {
                    PanelUsers.Controls.Remove(userControl);
                }
            }
            else if (((Button)sender).Name == "buttonDel")
            {
                PanelUsers.Controls.Remove(userControl);
                UpdateCompletedCount();
                UpdateAverageCompletionTime();
            }
        }

        private void UpdateCompletedCount()
        {
            int completedCount = CalculateCompletedRequests();
            Amounttextbox.Text = completedCount.ToString();
        }

        private void UserControl_StatusChanged(object sender, EventArgs e)
        {

            UpdateCompletedCount();
            UpdateAverageCompletionTime();
        }

        private int CalculateCompletedRequests()
        {
            int completedCount = 0;
            foreach (Control control in PanelUsers.Controls)
            {
                if (control is RecordsUserControl userControl && userControl.Status == "Выполнено")
                {
                    completedCount++;

                }
            }
            return completedCount;
        }

        private void UpdateAverageCompletionTime()
        {
            TimeSpan totalCompletionTime = TimeSpan.Zero;
            int completedRequestCount = 0;

            foreach (Control control in PanelUsers.Controls)
            {
                if (control is RecordsUserControl userControl && userControl.Status == "Выполнено")
                {
                    totalCompletionTime += DateTime.Now - userControl.CompletionTime;
                    completedRequestCount++;
                }
            }

            if (completedRequestCount > 0)
            {
                TimeSpan averageCompletionTime = totalCompletionTime / completedRequestCount;
                TimeAVG.Text = averageCompletionTime.ToString();
            }
            else
            {
                TimeAVG.Text = "0";
            }
        }

    }
}