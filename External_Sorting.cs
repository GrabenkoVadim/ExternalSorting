using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;

namespace KP
{
    public partial class ExternalSorting : Form
    {
        private int _accessesAmount;
        private bool _isSorted;
        private bool _isGenerated;
        private string _outputFile;
        private readonly WorkWithFile _workWithFile;

        public ExternalSorting(WorkWithFile workWithFile)
        {
            _workWithFile = workWithFile;
            InitializeComponent();
            From_List.Text = @"-10000";
            To_List.Text = @"10000";
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private void Generation_Button_Click(object sender, EventArgs e)
        {
            try
            {
                string sizeValue = Elements_Input_Field.Text;
                string minInput = From_List.Text;
                string maxInput = To_List.Text;
                bool sizeCheck = sizeValue.Any(c => !char.IsDigit(c) && c != '-');
                bool minCheck = minInput.Any(c => !char.IsDigit(c) && c != '-');
                bool maxCheck = maxInput.Any(c => !char.IsDigit(c) && c != '-');
                if (!sizeCheck && sizeValue != "" && !minCheck && !maxCheck && minInput != "" && maxInput != "")
                {
                    int value = int.Parse(sizeValue);
                    int minValue = int.Parse(From_List.Text);
                    int maxValue = int.Parse(To_List.Text);
                    if (IsValid(value, minValue, maxValue))
                    {
                        _workWithFile.SetupFile(value, minValue, maxValue);
                        Input_list.Text = _workWithFile.getInput_List();
                    }

                    _isGenerated = true;
                    _isSorted = false;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(@"Incorrect input!" + '\n' +
                                @"Value should be positive and less than 150_000_000!" + '\n' +
                                @"Range must be between -150_000_000 and 150_000_000");
            }
        }

        private bool IsValid(int value, int min, int max)
        {
            if (value is > 0 and <= 150_000_000 && min is >= -150_000_000 and <= 150_000_000 &&
                max is >= -150_000_000 and <= 150_000_000 && min < max)
            {
                return true;
            }

            return false;
        }


        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private void Sort_Button_Click(object sender, EventArgs e)
        {
            int value = int.Parse(Elements_Input_Field.Text);
            _outputFile = _workWithFile.OutputFileMaker(value);
            if (_workWithFile.GetInputFilePath() != null)
            {
                if (Balanced_Multiway_Merge_Button.Checked || Natural_Merge_Button.Checked ||
                    Polyphase_Sort_Button.Checked)
                {
                    if (!_isSorted)
                    {
                        if (Balanced_Multiway_Merge_Button.Checked)
                        {
                            InitSorting(new BalancedMultiwayMerging());
                        }

                        if (Natural_Merge_Button.Checked)
                        {
                            InitSorting(new NaturalMergeSort());
                        }

                        if (Polyphase_Sort_Button.Checked)
                        {
                            InitSorting(new PolyphaseSorting());
                        }

                        _isSorted = true;
                    }
                    else
                    {
                        MessageBox.Show(@"FILE IS ALREADY SORTED!");
                    }
                }
                else
                {
                    MessageBox.Show(@"PLEASE CHOOSE METHOD!");
                }
            }
            else

            {
                MessageBox.Show(@"PLEASE GENERATE FILE!");
            }
        }

        private void InitSorting(ISorting sorting)
        {
            sorting.Sort(_workWithFile.GetInputFilePath(), _outputFile);
            Output_list.Text = sorting.GetPartOfOutputFileElements();
            _accessesAmount = sorting.GetDiskReadCount() +
                              sorting.GetDiskWriteCount();
            NumberOfAccessesList.Text = _accessesAmount.ToString();
        }

        private void Open_Input_File_Button_Click(object sender, EventArgs e)
        {
            if (_isGenerated)
            {
                Process.Start(_workWithFile.GetInputFilePath());
            }
            else
            {
                MessageBox.Show(@"FILE DOES NOT EXIST!" + '\n' + @"PLEASE GENERATE FILE!");
            }
        }

        private void Open_Output_File_Button_Click(object sender, EventArgs e)
        {
            if (_isSorted)
            {
                Process.Start(_outputFile);
            }
            else if (_workWithFile.GetInputFilePath() != null)
            {
                MessageBox.Show(@"PLEASE SORT FILE!");
            }
            else
            {
                MessageBox.Show(@"FILE DOES NOT EXIST!" + '\n' + @"PLEASE GENERATE FILE!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Input_list_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}