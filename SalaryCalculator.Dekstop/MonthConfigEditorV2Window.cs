﻿using Console;
using FileTranslator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalaryCalculator.Dekstop
{
    public enum ErrorType
    {
        BadHoursConfig,
        BadInputFormat,
        SameValue,
    }
    public partial class MonthConfigEditorV2Window : Form
    {
        public delegate void methodHandler(MonthConfigEditorV2Window obj);
        public methodHandler RestartMonthConfigEditorV2Window;
        private readonly System.Drawing.Font _defaultMonthLabel = new System.Drawing.Font
            ("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
        private readonly Padding _defaultPadding = new Padding(0, 1, 0, 6);
        private MonthsWorkingHours _monthsWorkingHours;
        private int[] _MonthWorkingHoursTabWithIntValue = new int[13];
        private int[] _TabWithNewMonthWorkingHours = new int[13];
        private string[] _MonthWorkingHoursTabWithStringValue = new string[13];        
        private string[] _TabWithBadTextBoxes = new string[13];
        private string _BadInputError;
        private string _BadHoursConfigError = "Niepoprawna liczba roboczogodzin.";
        private string _SameValueError = "Liczba roboczogodzin jest taka sama.";

        public MonthConfigEditorV2Window(MonthsWorkingHours monthsWorkingHours, ConfigurationWindowMode status)
        {
            _monthsWorkingHours = monthsWorkingHours;
            InitializeComponent();
            if (status == ConfigurationWindowMode.ActualConfiguration) 
            {
                CreateLabelsAndFillItWithNumberOfMonthWorkingHours();
            }   else
            {
                CreateLabelsAndFillItWithNumberOfMonthWorkingHours();
                MessageBox.Show($"{Properties.Resources.succesfullSave_message}", "Info");
            }       
        }
        
        private void CreateLabelsAndFillItWithNumberOfMonthWorkingHours()
        {
            var monthsLabels = Enumerable
            .Range(1, 12)
            .Select(index => new Label
            {
                Text = _monthsWorkingHours[index].ToString(),
                Font = _defaultMonthLabel,

                Margin = _defaultPadding
            });
            foreach (var monthsLabel in monthsLabels)
            {
                flowLayoutPanel1.Controls.Add(monthsLabel);
            }
        }

        private void WriteTextBoxeValuesToTabWithStringValue()
        {
            _MonthWorkingHoursTabWithStringValue[1] = January_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[2] = February_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[3] = March_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[4] = April_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[5] = May_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[6] = June_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[7] = July_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[8] = August_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[9] = September_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[10] = October_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[11] = November_textBox.Text;
            _MonthWorkingHoursTabWithStringValue[12] = December_textBox.Text;
        }

        private void RemoveZeroValuesInTextBox()
        {
            WriteTextBoxeValuesToTabWithStringValue();
            for (int i = 1; i <= 12; i++)
            {
                if (_MonthWorkingHoursTabWithStringValue[i].IndexOf("0") == 0)
                {
                    try
                    {
                        Parser.ParseStringToInt(_MonthWorkingHoursTabWithStringValue[i]);
                        ClearTextBoxes(i);
                        _MonthWorkingHoursTabWithStringValue[i] = "";
                    }
                    catch
                    {
                    }
                }
            }
        }

        private bool CheckIfChangesHaveBeenMadeInTextBoxes()
        {
            WriteTextBoxeValuesToTabWithStringValue();
            RemoveZeroValuesInTextBox();
            CountEmptyTextBoxes();
            return CountEmptyTextBoxes() == 12 ? false : true;
        }

        private void TryAllocateNonEmptyValueFromStringTabToIntTab()
        {
            for (int i = 1; i <= 12; i++)
            {
                try
                {
                    if(_MonthWorkingHoursTabWithStringValue[i] != "")
                    {
                        _MonthWorkingHoursTabWithIntValue[i] =
                            Parser.ParseStringToInt(_MonthWorkingHoursTabWithStringValue[i]);
                    }
                }
                catch (Exception e)
                {
                    RefillTabWithWrongTextBoxesValues(i, ErrorType.BadInputFormat);
                    _BadInputError = e.Message;
                }
            }
        }
        
        private void CheckTextBoxesForTheSameValues()
        {
            for (int i = 1; i <= 12; i++)
            {
                if(_MonthWorkingHoursTabWithIntValue[i] == _monthsWorkingHours[i])
                {
                    RefillTabWithWrongTextBoxesValues(i, ErrorType.SameValue);
                }
            }
        }

        private void CheckInvalidMonthConfigInTabWithIntValue()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (_MonthWorkingHoursTabWithIntValue[i] > 0)
                {
                    if (_MonthWorkingHoursTabWithIntValue[i] < 100 ||
                        _MonthWorkingHoursTabWithIntValue[i] > 200)
                    {
                        RefillTabWithWrongTextBoxesValues(i, ErrorType.BadHoursConfig);
                    }
                }
                else if (_MonthWorkingHoursTabWithIntValue[i] < 0)
                {
                    RefillTabWithWrongTextBoxesValues(i, ErrorType.BadHoursConfig);
                }
            }
        }

        private int CountEmptyTextBoxes()
        {
            var EmptyTextBoxes = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (_MonthWorkingHoursTabWithStringValue[i] == "")
                {
                     EmptyTextBoxes++;
                }
            }
            return EmptyTextBoxes;
        }        

        private void RefillTabWithWrongTextBoxesValues(int order, ErrorType errorType)
        {
            _TabWithBadTextBoxes[order] = errorType.ToString();
        }

        private void IdentifyErrorTypeInTabWithBadTextBoxes()
        {
            string badInputError = "", badHoursConfigError = "", sameValueError = "";
            for (int i = 1; i <= 12; i++)
            {
                if (_TabWithBadTextBoxes[i] == ErrorType.BadHoursConfig.ToString())
                {
                    SetBackColorInSelectedTextBox(i, Color.Yellow);
                    badHoursConfigError = _BadHoursConfigError;
                }
                if (_TabWithBadTextBoxes[i] == ErrorType.BadInputFormat.ToString())
                {
                    SetBackColorInSelectedTextBox(i, Color.Red);
                    badInputError = _BadInputError;
                }
                if (_TabWithBadTextBoxes[i] == ErrorType.SameValue.ToString())
                {
                    SetBackColorInSelectedTextBox(i, Color.LightBlue);
                    sameValueError = _SameValueError;
                }
            }
            ShowSpecialMessageBox(badInputError, badHoursConfigError, sameValueError);
        }

        private void ShowSpecialMessageBox(string badInputError, string badHoursConfigError, string sameValueError)
        {
            var messageBoxForEditorV2 = new MessageBoxForEditorV2();
            messageBoxForEditorV2.PrintMessage(badInputError, badHoursConfigError, sameValueError);
            messageBoxForEditorV2.Location = new Point(this.Right, this.Top + 150);
            messageBoxForEditorV2.StartPosition = FormStartPosition.Manual;
            messageBoxForEditorV2.ShowDialog();
        }
        private bool CheckIfSaveIsPossible()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (_TabWithBadTextBoxes[i] != "")
                {
                    return false;
                }
            }
            return true;
        }

        private void RefillTabWithNewMonthWorkingHours()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (_MonthWorkingHoursTabWithIntValue[i] == 0)
                {
                    _TabWithNewMonthWorkingHours[i] = _monthsWorkingHours[i];
                } else
                {
                    _TabWithNewMonthWorkingHours[i] = _MonthWorkingHoursTabWithIntValue[i];
                }
            }            
        }

        private void WriteNewValueToSelectedMonthConfigFromTab(MonthConfigPaths monthConfigPath)
        {
            Dictionary<int, int> contents = new Dictionary<int, int>
               {
                 { 1, _TabWithNewMonthWorkingHours[1] },
                 { 2, _TabWithNewMonthWorkingHours[2] },
                 { 3, _TabWithNewMonthWorkingHours[3] },
                 { 4, _TabWithNewMonthWorkingHours[4] },
                 { 5, _TabWithNewMonthWorkingHours[5] },
                 { 6, _TabWithNewMonthWorkingHours[6] },
                 { 7, _TabWithNewMonthWorkingHours[7] },
                 { 8, _TabWithNewMonthWorkingHours[8] },
                 { 9, _TabWithNewMonthWorkingHours[9] },
                 { 10, _TabWithNewMonthWorkingHours[10] },
                 { 11, _TabWithNewMonthWorkingHours[11] },
                 { 12, _TabWithNewMonthWorkingHours[12] },
                };
            JsonFileTranslator.WriteTo($"{monthConfigPath.ToString()}.json", contents);
        } 

        private void SetBackColorInSelectedTextBox(int TextBoxNumber, Color color)
        {
            switch (TextBoxNumber)
            {
                case 1:
                    January_textBox.BackColor = color;
                    break;
                case 2:
                    February_textBox.BackColor = color;
                    break;
                case 3:
                    March_textBox.BackColor = color;
                    break;
                case 4:
                    April_textBox.BackColor = color;
                    break;
                case 5:
                    May_textBox.BackColor = color;
                    break;
                case 6:
                    June_textBox.BackColor = color;
                    break;
                case 7:
                    July_textBox.BackColor = color;
                    break;
                case 8:
                    August_textBox.BackColor = color;
                    break;
                case 9:
                    September_textBox.BackColor = color;
                    break;
                case 10:
                    October_textBox.BackColor = color;
                    break;
                case 11:
                    November_textBox.BackColor = color;
                    break;
                case 12:
                    December_textBox.BackColor = color;
                    break;
                default:
                    for(int i = 0; i <= 11; i++)
                    {
                        SetBackColorInSelectedTextBox(i + 1, color);
                    }
                    break;
            }
        }

        private void ClearTabs()
        {            
            for (int i = 1; i <= 12; i++)
            {
                _MonthWorkingHoursTabWithIntValue[i] = 0;
                _MonthWorkingHoursTabWithStringValue[i] = "";
                _TabWithBadTextBoxes[i] = "";
                _TabWithNewMonthWorkingHours[i] = 0;
            }
        }

        private void ClearTextBoxes(int TextBoxNumber)
        {
            switch (TextBoxNumber)
            {
                case 1:
                    January_textBox.Text = "";
                    break;
                case 2:
                    February_textBox.Text = "";
                    break;
                case 3:
                    March_textBox.Text = "";
                    break;
                case 4:
                    April_textBox.Text = "";
                    break;
                case 5:
                    May_textBox.Text = "";
                    break;
                case 6:
                    June_textBox.Text = "";
                    break;
                case 7:
                    July_textBox.Text = "";
                    break;
                case 8:
                    August_textBox.Text = "";
                    break;
                case 9:
                    September_textBox.Text = "";
                    break;
                case 10:
                    October_textBox.Text = "";
                    break;
                case 11:
                    November_textBox.Text = "";
                    break;
                case 12:
                    December_textBox.Text = "";
                    break;
                default:
                    for(int i = 0; i <= 11; i++)
                    {
                        ClearTextBoxes(i + 1);
                    }
                    break;
            }            
        }

        private void Save_StripMenu_Click_1(object sender, EventArgs e)
        {
            ClearTabs();
            SetBackColorInSelectedTextBox(13, Color.White);
            if (CheckIfChangesHaveBeenMadeInTextBoxes())
            {
                TryAllocateNonEmptyValueFromStringTabToIntTab();
                CheckInvalidMonthConfigInTabWithIntValue();
                CheckTextBoxesForTheSameValues();
                if (CheckIfSaveIsPossible())
                {
                    RefillTabWithNewMonthWorkingHours();
                    WriteNewValueToSelectedMonthConfigFromTab(MonthConfigPaths.MonthConfig);
                    WriteNewValueToSelectedMonthConfigFromTab(MonthConfigPaths.MonthConfigLastGoodConfiguration);
                    RestartMonthConfigEditorV2Window?.Invoke(this);
                } else
                {
                    IdentifyErrorTypeInTabWithBadTextBoxes();
                }                
            } else
            {                
                MessageBox.Show(Properties.Resources.noChangesMade_message, "Info");
                ClearTextBoxes(13);
            }
        }

        private void Reset_StripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTabs();
            ClearTextBoxes(13);
            SetBackColorInSelectedTextBox(13, Color.White);
        }

        private void Exit_StripMenu_Click(object sender, EventArgs e)
        {
            ClearTabs();
            SetBackColorInSelectedTextBox(13, Color.White);
            if (CheckIfChangesHaveBeenMadeInTextBoxes())
            {
                if (MessageBox.Show("Wprowadzono zmiany, czy chcesz je zapisać?", "Uwaga!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TryAllocateNonEmptyValueFromStringTabToIntTab();
                    CheckInvalidMonthConfigInTabWithIntValue();
                    CheckTextBoxesForTheSameValues();
                    if (CheckIfSaveIsPossible())
                    {
                        RefillTabWithNewMonthWorkingHours();
                        WriteNewValueToSelectedMonthConfigFromTab(MonthConfigPaths.MonthConfig);
                        WriteNewValueToSelectedMonthConfigFromTab(MonthConfigPaths.MonthConfigLastGoodConfiguration);
                        RestartMonthConfigEditorV2Window?.Invoke(this);
                    }
                    else
                    {
                        IdentifyErrorTypeInTabWithBadTextBoxes();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}