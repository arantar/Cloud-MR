using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_MR
{
    class Matrix : IEnumerable
    {
        private int rows;
        private int colums;
        private float[,] _matrix;
        public float[,] _Matrix {
            get { return _matrix; }
            set { _matrix = value; }
        }
        public int Rows {
            get { return rows; }
            set { rows = value; }
        }
        public int Colums {
            get { return colums; }
            set { colums = value; }
        }
        public float this[int i, int j] {
            get { return _matrix[i, j]; }
            set { _matrix[i, j] = value; }
        }
        public IEnumerator GetEnumerator()
        {
            return _Matrix.GetEnumerator();
        }
        public Matrix() { }
        public Matrix(int rows, int colums)
        {
            Rows = rows;
            Colums = colums;
            _matrix = new float[Rows, Colums];
        }
        public Matrix(float[,] matrix)
        {
            this._matrix = matrix;
            Rows = matrix.GetLength(0);
            Colums = matrix.GetLength(1);
        }
        public static Matrix operator +(Matrix A, Matrix B)
        {
            if (!CheckSquareMatrix(A) || !CheckSquareMatrix(B)) {
                throw new Exception("При сложении матрицы должны быть квадратными");
            }
            Matrix addition = new Matrix(A.Rows, A.Colums);
            for (int i = 0; i < A.Rows; i++) {
                for (int j = 0; j < A.Colums; j++) {
                    addition[i, j] = A[i, j] + B[i, j];
                }
            }
            return addition;
        }
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if (!CheckSquareMatrix(A) || !CheckSquareMatrix(B)) {
                throw new Exception("При вычитании матрицы должны быть квадратными");
            }
            Matrix subtraction = new Matrix(A.Rows, A.Colums);
            for (int i = 0; i < A.Rows; i++) {
                for (int j = 0; j < A.Colums; j++) {
                    subtraction[i, j] = A[i, j] - B[i, j];
                }
            }
            return subtraction;
        }
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.Colums != B.Rows) {
                throw new Exception("Матрицы не согласованы");
            }
            Matrix multiplication = new Matrix(A.Rows, B.Colums);
            for (int i = 0; i < A.Rows; i++) {
                for (int j = 0; j < B.Colums; j++) {
                    for (int k = 0; k < A.Rows; k++) {
                        multiplication[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return multiplication;
        }
        public static Matrix TransposeMatrix(float[,] matrix)
        {
            Matrix transposeMatrix = new Matrix(matrix);
            for (int i = 0; i < transposeMatrix.Rows; i++) {
                for (int j = 0; j < transposeMatrix.Colums; j++) {
                    transposeMatrix[i, j] = matrix[j, i];
                }
            }
            return transposeMatrix;
        }
        public static Matrix MatrixInverse(float[,] matrix)
        {
            if (!CheckSquareMatrix(matrix)) {
                throw new Exception("Для нахождения обратной матрицы искомая матрица должна быть квадратной");
            }
            int n = matrix.GetLength(0);
            float[] X;
            int[,] P;
            float[,] luMatrix = LUP_Decompose(matrix, out P);
            Matrix result = new Matrix(n, n);
            for (int i = 0; i < n; i++) {
                X = LUP_Solve(luMatrix, P, i);
                for (int j = 0; j < n; j++) {
                    result[j, i] = X[j];
                }
            }
            return result;
        }
        private static float[,] LUP_Decompose(float[,] matrix, out int[,] P)
        {
            float[,] luMatrix = MatrixDuplicate(matrix);
            int n = luMatrix.GetLength(0);
            P = new int[n, n];
            for (int i = 0; i < n; i++) {
                P[i, i] = 1;
            }
            for (int i = 0; i < n; i++) {
                double pivotValue = 0;
                int pivot = -1;
                for (int row = i; row < n; row++) {
                    if (Math.Abs(luMatrix[row, i]) > pivotValue) {
                        pivotValue = Math.Abs(luMatrix[row, i]);
                        pivot = row;
                    }
                }
                if (pivotValue != 0) {
                    SwapRows(pivot, i, P);
                    SwapRows(pivot, i, luMatrix);
                    for (int j = i + 1; j < n; j++) {
                        luMatrix[j, i] /= luMatrix[i, i];
                        for (int k = i + 1; k < n; k++)
                            luMatrix[j, k] -= luMatrix[j, i] * luMatrix[i, k];
                    }
                }
            }
            return luMatrix;
        }
        private static float[] LUP_Solve(float[,] luMatrix, int[,] P, int colum_P)
        {
            int n = luMatrix.GetLength(0);
            float[] X = new float[n];
            for (int i = 0; i < n; i++) {
                X[i] = P[i, colum_P];
            }
            for (int i = 1; i < n; i++) {
                float sum = X[i];
                for (int j = 0; j < i; j++)
                    sum -= luMatrix[i, j] * X[j];
                X[i] = sum;
            }
            X[n - 1] /= luMatrix[n - 1, n - 1];
            for (int i = n - 2; i >= 0; i--) {
                float sum = X[i];
                for (int j = i + 1; j < n; j++)
                    sum -= luMatrix[i, j] * X[j];
                X[i] = sum / luMatrix[i, i];
            }
            return X;
        }
        private static void SwapRows(int pivot, int index, int[,] matrix)
        {
            int temp = 0;
            for (int j = 0; j < matrix.GetLength(1); j++) {
                temp = matrix[pivot, j];
                matrix[pivot, j] = matrix[index, j];
                matrix[index, j] = temp;
            }
        }
        private static void SwapRows(int pivot, int index, float[,] matrix)
        {
            float temp = 0;
            for (int j = 0; j < matrix.GetLength(1); j++) {
                temp = matrix[pivot, j];
                matrix[pivot, j] = matrix[index, j];
                matrix[index, j] = temp;
            }
        }
        private static float[,] MatrixDuplicate(float[,] matrix)
        {
            float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] = matrix[i, j];
            return result;
        }
        private static bool CheckSquareMatrix(Matrix matrix)
        {
            if (matrix.Rows == matrix.Colums) { return true; }
            else { return false; }
        }
        private static bool CheckSquareMatrix(float[,] matrix)
        {
            if (matrix.GetLength(0) == matrix.GetLength(1)) { return true; }
            else { return false; }
        }
    }
}