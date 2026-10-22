using System.Data;
using System.Text;

namespace Matrices;

public abstract class Matrix
{
    public int Rows { get; set; }
    public int Columns { get; set; }

    public Matrix(int rows, int cols)
    {
        this.Columns = cols;
        this.Rows = rows;
    }

    public abstract void Identity();
    public abstract Matrix Transpose();
    public abstract float Norm();
    public abstract float this[int row,int col]
    {
        get;
        set;
    }

    public override string ToString()
    {
        StringBuilder sb = new  StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                string temp = Convert.ToString(this[i,j]);
                sb.Append($"{temp} ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

public class SparseMatrix : Matrix
{
    public Dictionary<Tuple<int,int>, float> Matrix;
    public SparseMatrix(int rows, int cols) : base(rows, cols)
    {
        Matrix = new Dictionary<Tuple<int,int>, float>();
    }

    public override float this[int row, int col]
    {
        get
        {
            if (Matrix.TryGetValue(Tuple.Create(row, col), out float value))
            {
                return value;
            }
            else
            {
                if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                {
                    return 0;
                }
                throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                Matrix[Tuple.Create(row, col)] = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override Matrix Transpose()
    {

        Matrix final = new SparseMatrix(Columns, Rows);
        foreach (var entry in Matrix)
        {
            final[entry.Key.Item2,entry.Key.Item1]=entry.Value;
        }
        return final;
    }

    public override float Norm()
    {
        float norm = 0;
        foreach(var item in Matrix.Values)
        {
            norm+=(float)Math.Pow(item,2);
        }
        return (float)Math.Sqrt(norm);

    }

    public override void Identity()
    {
        this.Matrix.Clear();
        for (int i = 0; i < Columns; i++)
        {
            this[i, i] = 0;
        }
    }
}

public class DenseMatrix : Matrix
{
    float[,] matrix;
    public DenseMatrix(int rows, int cols) : base(rows, cols)
    {
        matrix = new float[rows, cols];
    }

    public override float this[int row, int col]
    {
        get
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                return matrix[row, col];
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                matrix[row, col]=value;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public override Matrix Transpose()
    {
        Matrix final = new DenseMatrix(Columns, Rows);
        for (int r = 0; r < Columns; r++)
        {
            for (int c = 0; c < Rows; c++)
            {
                final[r,c] = this[c,r];
            }
        }
        return final;
    }

    public override float Norm()
    {
        float norm = 0;
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                norm+=((float)Math.Pow(this[r,c],2));
            }
        }
        return (float)Math.Sqrt(norm);
    }

    public override void Identity()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                this[r,c] = 0;
            }
        }

        for (int i = 0; i < Rows; i++)
        {
            this[i, i] = 0;
        }
    }
}