using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        const int InputNumbers = 0;
        const int InputAlternative = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // С любой новой строки/с любой новой строки от любой позиции(обозначенной пробелами, символами Tab) вводится число.
            // Цифры в числе пишутся слитно, после числа с любого пробела/символа Tab вводятся альтернативы.
            // Альтернатива может отделяться от другой любым количеством пробелов, символов Tab, знаков '>', и любым количеством пробелов, символов Tab и знаков '>',
            // расположенных как угодно.
            int InputMode = InputNumbers;

            bool bFirst = true;
            bool bErrorNum = true;
            bool bErrorAlt = true;
            string text = "";
            string alternatives = "";
            string numbers = "0123456789";
            string symbols = "> \r\0	";
            int[] digits = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int i, j, n, f, ff = 0, oldf = 0;
            int c = 1;
            int[] nums;
            int CountAlt = 0;

            text = textBox1.Text;
            int l = text.Length;
            if(l == 0)
            {
                MessageBox.Show("Задача задана неверно", "Ошибка");
                return;
            }

            for (i = 0; i < l; i++)
            {
                if (text[i] == symbols[2]) c++;
                while (text[i] == symbols[2])
                {
                    i += 2;
                    if (i == l) break;
                    while ((text[i] == symbols[1]) || (text[i] == symbols[4]))
                    {
                        i++;
                        if (i == l) break;
                    }
                }
                if (i == l) break;
            }

            nums = new int[c];

            c = 0;
            i = 0;
            while (true)
            {
                if (i == 0) goto label1;

                if ((text[i] == symbols[1]) || (text[i] == symbols[4]))
                {
                    InputMode = InputAlternative;
                    i++;
                    if (i == l) goto label4; // если достигли границы массива text, то выйти
                    goto label1;
                }

                if (text[i] == symbols[2])
                {
                    //в этом случае text[i] это "\r" перед text[i+1] = "\n"
                    InputMode = InputNumbers;
                    i += 2;
                    if (i == l) goto label4; // если достигли границы массива text, то выйти
                }
label1:
                if (InputMode == InputNumbers)
                {
                    while (text[i] == symbols[2])
                    {
                        i += 2;
                        if (i == l) goto label4; // если достигли границы массива text, то выйти
                    }
                    while ((text[i] == symbols[1]) || (text[i] == symbols[4]))
                    {
                        i++;
                        if (i == l) goto label4; // если достигли границы массива text, то выйти
                    }

                    j = 0;
                    while (j < 10)
                    {
                        if (text[i] == numbers[j])
                        {
                            nums[c] = nums[c] * 10 + digits[j];
                            i++;
                            if (i == l) goto label4; // если достигли границы массива text, то выйти
                            j = -1;
                            bErrorNum = false;
                        }

                        j++;
                    }
                    if ((text[i] == symbols[2]) && !bErrorNum)
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                    if ((text[i] == symbols[2]) && bErrorNum)
                    {
                        goto label2;
                    }
                    if ((text[i] != symbols[1]) && (text[i] != symbols[4]))
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                    c++;
label2:
                    bErrorNum = true;
                }
                if (InputMode == InputAlternative)
                {
                    while (true)
                    {
label3:
                        if (text[i] == symbols[2])
                        {
                            if (bErrorAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка"); // если в массив alternatives не удалось записать
                                // первую ранжировку(а если по логике алгоритма - хоть одну ранжировку)
                                return;
                            }
                            break;
                        }

                        if ((text[i] == symbols[0]) || (text[i] == symbols[1]) || (text[i] == symbols[4]))
                        {
                            i++;
                            if (i == l)
                            {
                                if (bErrorAlt)
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка"); // если в массив alternatives не удалось записать
                                    // первую ранжировку(а если по логике алгоритма - хоть одну ранжировку)
                                    return;
                                }
                                if ((oldf - ff) != CountAlt)
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                goto label4; // если достигли границы массива text, то выйти
                            }
                            goto label3;
                        }

                        if (!bFirst)
                        {
                            bErrorAlt = true;
                            for (n = 0; n < CountAlt; n++)
                            {
                                if (text[i] == alternatives[n])
                                {
                                    bErrorAlt = false;
                                    break;
                                }
                            }

                            if (bErrorAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка");
                                return;
                            }

                            f = oldf;
                            while (f != ff)
                            {
                                if (alternatives[f-1] == text[i])
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                f--;
                            }
                            if ((oldf - ff) == CountAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка");
                                return;
                            }
                            alternatives += text[i];
                            oldf++;
                        }
                        else
                        {
                            f = CountAlt;
                            while (f != 0)
                            {
                                if (alternatives[f-1] == text[i])
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                f--;
                            }
                            alternatives += text[i];
                            oldf++;
                            CountAlt++;
                            bErrorAlt = false;
                        }

                        i++;
                        if (i == l)
                        {
                            if ((oldf - ff) != CountAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка");
                                return;
                            }
                            goto label4; // если достигли границы массива text, то выйти
                        }
                    }

                    if ((oldf - ff) != CountAlt)
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                    bFirst = false;
                    alternatives += " ";
                    oldf++;
                    ff = oldf;
                }
            }
label4:
            if(alternatives.Length == 0)
            {
                MessageBox.Show("Задача задана неверно", "Ошибка");
                return;
            }
            if(alternatives[alternatives.Length - 1] == symbols[1])
            {
                alternatives = alternatives.Remove(alternatives.Length - 1);
            }
            textBox2.Text += "Все альтернативы(по условию):\r\n" + alternatives + "\r\n";
            alternatives += symbols[1];

            n  = 0;
            i  = 0;
            j  = 0;
            f  = 0;
            ff = 0;
            c  = 0;
            l  = alternatives.Length;
            int oldi = 0;
            bFirst = true;
            bool bSecond = true;
            while (n < CountAlt)
            {
                textBox2.Text += "Показатель предпочтительности для альтернативы " + alternatives[n] + ":\r\n";
                while (alternatives[ff] != symbols[1])
                {
                    while (l > i*(CountAlt + 1) + ff)
                    {
                        if (alternatives[i*(CountAlt + 1) + ff] == alternatives[n])
                        {
                            if(!bFirst && bSecond)
                            {
                                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - nums[oldi].ToString().Length - 1);
                                textBox2.Text += "(" + nums[oldi].ToString() + "+";
                                bSecond = false;
                            }
                            j += nums[i];
                            textBox2.Text += nums[i].ToString() + "+";
                            oldi = i;
                            bFirst = false;
                        }
                        i++;
                    }
                    c = CountAlt - ff;
                    if (j != 0)
                    {
                        f += j * c;
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
                        if (bSecond) textBox2.Text += "*";
                        else textBox2.Text += ")*";
                        textBox2.Text += c.ToString() + " + ";
                    }
                    ff++;
                    i = 0;
                    j = 0;
                    bFirst = true;
                    bSecond = true;
                }
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 2);
                textBox2.Text += "= " + f.ToString() + "\r\n";
                f = 0;
                ff = 0;
                n++;
            }
        }
    }
}
