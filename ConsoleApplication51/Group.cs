using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication51
{
    public class Group
    {
        public int m;
        public bool isIsolated;
        public bool isAssociative;
        public string identityInfo;
        public string inverseElementsInfo;
        public EquivalenceClass multiplyIdentity = null;
        public EquivalenceClass additionIdentity = null;

        public List<EquivalenceClass> classesList = new List<EquivalenceClass>();

        public EquivalenceClass this[int num]
        {
            get
            {
                return classesList.Find(e => e.remainder == num);
            }
        }

        public Group(int m)
        {
            this.m = m;

            WeakReference weakRef = new WeakReference(this);

            for (int i = 0; i < m; i++)
            {
                classesList.Add(new EquivalenceClass(weakRef, i));
            }

            getIdentityInfo();
            getIsolationInfo();
            getAssociationInfo();
            getInverseElementsInfo();
        }

        public void analyse()
        {
            Console.WriteLine($"\nGroup M" + m + " analysis:");
            Console.WriteLine(ToString());
            showOperations();
            Console.WriteLine($"\nSet has closure under operations: {isIsolated}");
            Console.WriteLine($"\nSet is associative: {isAssociative}");
            Console.WriteLine(identityInfo);
            Console.WriteLine(inverseElementsInfo);
        }

        public void getInverseElementsInfo()
        {

            if (isIsolated)
            {
                string desc = "\nInverse elements for + operation:\n";

                for (int i = 0; i < this.classesList.Count; i++)
                {
                    for (int j = 0; j < this.classesList.Count; j++)
                    {
                        EquivalenceClass a = this.classesList[i];
                        EquivalenceClass b = this.classesList[j];

                        EquivalenceClass result = a + b;

                        if (result == additionIdentity)
                        {
                            desc += $"\n{a.remainder} + {b.remainder} = {result.remainder}";
                        }
                    }
                }

                desc += "\n\nInverse elements for * operation:\n";

                for (int i = 0; i < this.classesList.Count; i++)
                {
                    for (int j = 0; j < this.classesList.Count; j++)
                    {
                        EquivalenceClass a = this.classesList[i];
                        EquivalenceClass b = this.classesList[j];

                        EquivalenceClass result = a * b;

                        if (result == multiplyIdentity)
                        {
                            desc += $"\n{a.remainder} * {b.remainder} = {result.remainder}";
                        }
                    }
                }

                inverseElementsInfo = desc;
            } else
            {
                inverseElementsInfo = "Set is has no closure under operations";
            } 
        }

        public void showOperationLog(int e1, int e2, int result, char operation)
        {
            Console.WriteLine($"{e1} {operation} {e2} = {result}       (m={m})");
        }

        public void showOperations()
        { 
            Console.WriteLine($"\nAdding:\n");

            for (int i = 0; i < this.classesList.Count; i++)
            {
                for (int j = 0; j < this.classesList.Count; j++)
                {
                    EquivalenceClass result = this.classesList[i] + this.classesList[j];
                    showOperationLog(this.classesList[i].remainder, this.classesList[j].remainder, result.remainder, '+');
                }
            }

            Console.WriteLine($"\nMultiplying:\n");

            for (int i = 0; i < this.classesList.Count; i++)
            {
                for (int j = 0; j < this.classesList.Count; j++)
                {
                    EquivalenceClass result = this.classesList[i] * this.classesList[j];
                    showOperationLog(this.classesList[i].remainder, this.classesList[j].remainder, result.remainder, '*');
                }
            }
        }
        public void getIsolationInfo()
        {
            bool isIsolated = true;

            for (int i = 0; i < this.classesList.Count; i++)
            {
                for (int j = 0; j < this.classesList.Count; j++)
                {
                    EquivalenceClass resultMultipy = this.classesList[i] * this.classesList[j];
                    EquivalenceClass resultReverseMultipy = this.classesList[j] * this.classesList[i];

                    EquivalenceClass resultAddition = this.classesList[i] * this.classesList[j];
                    EquivalenceClass resultReverseAddition = this.classesList[j] * this.classesList[i];

                    isIsolated = isResultIsolated(resultAddition) && isResultIsolated(resultReverseAddition) && isResultIsolated(resultMultipy) && isResultIsolated(resultReverseMultipy);
                    if (!isIsolated)
                    {
                        break;
                    }
                }
                if (!isIsolated)
                {
                    break;
                }
            }
            this.isIsolated = isIsolated;
        }

        public void getAssociationInfo()
        {
            bool hasAssociativeProperty = true;

            for (int i = 0; i < this.classesList.Count; i++)
            {
                for (int j = 0; j < this.classesList.Count; j++)
                {
                    for (int h = 0; h < this.classesList.Count; h++)
                    {
                        EquivalenceClass a = this.classesList[i];
                        EquivalenceClass b = this.classesList[j];
                        EquivalenceClass c = this.classesList[h];

                        EquivalenceClass result1 = (a * b) * c;
                        EquivalenceClass result2 = a * (b * c);

                        if (result1.remainder != result2.remainder)
                        {
                            hasAssociativeProperty = false;
                            break;
                        }
                    }
                    if (!hasAssociativeProperty)
                    {
                        break;
                    }
                }
                if (!hasAssociativeProperty)
                {
                    break;
                }
            }
            this.isAssociative = hasAssociativeProperty;
        }
        public void getIdentityInfo()
        {
            List<EquivalenceClass> leftIdentityMultiply = new List<EquivalenceClass>();
            List<EquivalenceClass> rightIdentityMultiply = new List<EquivalenceClass>();

            List<EquivalenceClass> leftIdentityAddition = new List<EquivalenceClass>();
            List<EquivalenceClass> rightIdentityAddition = new List<EquivalenceClass>();

            for (int i = 0; i < this.classesList.Count; i++)
            {
                for (int j = 0; j < this.classesList.Count; j++)
                {
                    EquivalenceClass a = this.classesList[i];
                    EquivalenceClass b = this.classesList[j];

                    EquivalenceClass resultMultipy = a * b;

                    if (resultMultipy.remainder == a.remainder)
                    {
                        if (isIdentity(b, '*', false) && !rightIdentityMultiply.Contains(b))
                        {
                            rightIdentityMultiply.Add(b);
                        }
                    }
                    else if (resultMultipy.remainder == b.remainder)
                    {
                        if (isIdentity(a, '*', true) && !leftIdentityMultiply.Contains(a))
                        {
                            leftIdentityMultiply.Add(a);
                        }
                    }

                    EquivalenceClass resultAddition = a + b;

                    if (resultAddition.remainder == a.remainder)
                    {
                        if (isIdentity(b, '+', false) && !rightIdentityAddition.Contains(b))
                        {
                            rightIdentityAddition.Add(b);
                        }
                    }
                    else if (resultAddition.remainder == b.remainder)
                    {
                        if (isIdentity(a, '+', true) && !leftIdentityAddition.Contains(a))
                        {
                            leftIdentityAddition.Add(a);
                        }
                    }
                }
            }

            string desc = "";

            if (leftIdentityAddition.Count == 1 && rightIdentityAddition.Count == 1 && leftIdentityAddition[0] == rightIdentityAddition[0])
            {
                desc += "\nGroup has two-sided identity for + operation: " + leftIdentityAddition[0].remainder;
                additionIdentity = leftIdentityAddition[0];
            }
            else if (leftIdentityAddition.Count == 0 && rightIdentityAddition.Count == 0)
            {
                desc += "\nGroup has no identities for + operation.";
            }
            else
            {
                desc += "\nIdentities for + operation:\nLeft: ";
                foreach (EquivalenceClass e in leftIdentityAddition)
                {
                    desc += e.remainder + " ";
                }
                desc += "\nRight: ";
                foreach (EquivalenceClass e in rightIdentityAddition)
                {
                    desc += e.remainder + " ";
                }
            }

            if (leftIdentityMultiply.Count == 1 && rightIdentityMultiply.Count == 1 && leftIdentityMultiply[0] == rightIdentityMultiply[0])
            {
                desc += "\nGroup has two-sided identity for * operation: " + leftIdentityMultiply[0].remainder;
                multiplyIdentity = leftIdentityMultiply[0];
            }
            else if (leftIdentityMultiply.Count == 0 && rightIdentityMultiply.Count == 0)
            {
                desc += "\nGroup has no identities for * operation.";
            }
            else
            {
                desc += "\nIdentities for * operation:\nLeft: ";
                foreach (EquivalenceClass e in leftIdentityMultiply)
                {
                    desc += e.remainder + " ";
                }
                desc += "\nRight: ";
                foreach (EquivalenceClass e in rightIdentityMultiply)
                {
                    desc += e.remainder + " ";
                }
            }

            identityInfo = desc;
        }
        public bool isIdentity(EquivalenceClass identity, char operation, bool isLeft)
        {
            for (int i = 0; i < this.classesList.Count; i++)
            {
                if (isLeft)
                {
                    if (operation == '+')
                    {
                        EquivalenceClass result = identity + this.classesList[i];
                        if (result.remainder != this.classesList[i].remainder)
                        {
                            return false;
                        }
                    }
                    else if (operation == '*')
                    {
                        EquivalenceClass result = identity * this.classesList[i];
                        if (result.remainder != this.classesList[i].remainder)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid operation");
                    }
                }
                else
                {
                    if (operation == '+')
                    {
                        EquivalenceClass result = this.classesList[i] + identity;
                        if (result.remainder != this.classesList[i].remainder)
                        {
                            return false;
                        }
                    }
                    else if (operation == '*')
                    {
                        EquivalenceClass result = this.classesList[i] * identity;
                        if (result.remainder != this.classesList[i].remainder)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid operation");
                    }
                }
            }
            return true;
        }

        public bool isResultIsolated(EquivalenceClass result)
        {
            return !(result.remainder < 0 || result.remainder >= m);
        }

        public override string ToString()
        {
            string desc = $"Group m={m}. Classes:\n";
            foreach(EquivalenceClass eClass in classesList)
            {
                desc += eClass.ToString();
            }

            return desc;
        }
    }
}
