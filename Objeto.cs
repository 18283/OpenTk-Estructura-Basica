using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;

namespace BasicOpenTK
{
    internal class Objeto
    {
        private int vertexBufferObject;
        private int elementBufferObject;
        private int vertexArrayObject;

        private static readonly float[] vertices = new float[]
        {
            // Cubo horizontal
            -0.50f,  0.30f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior izquierdo frontal
             0.50f,  0.30f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior derecho frontal
             0.50f,  0.00f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior derecho frontal
            -0.50f,  0.00f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior izquierdo frontal

            -0.50f,  0.30f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior izquierdo trasero
             0.50f,  0.30f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior derecho trasero
             0.50f,  0.00f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior derecho trasero
            -0.50f,  0.00f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior izquierdo trasero

            // Cubo vertical
            -0.20f,  0.00f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior izquierdo frontal
             0.20f,  0.00f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior derecho frontal
             0.20f, -0.80f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior derecho frontal
            -0.20f, -0.80f,  0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior izquierdo frontal

            -0.20f,  0.00f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior izquierdo trasero
             0.20f,  0.00f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Superior derecho trasero
             0.20f, -0.80f, -0.15f,  1.0f, 0.57f, 0.50f,  // Vértice Inferior derecho trasero
            -0.20f, -0.80f, -0.15f,  1.0f, 0.57f, 0.50f   // Vértice Inferior izquierdo trasero
        };

        private static readonly uint[] indices = new uint[]
        {
            // Cubo horizontal
            0, 1, 2, 0, 2, 3,    // Cara frontal
            4, 5, 6, 4, 6, 7,    // Cara trasera
            0, 4, 7, 0, 7, 3,    // Cara izquierda
            1, 5, 6, 1, 6, 2,    // Cara derecha
            0, 1, 5, 0, 5, 4,    // Cara superior
            3, 2, 6, 3, 6, 7,    // Cara inferior

            // Cubo vertical
            8, 9, 10, 8, 10, 11, // Cara frontal
            12, 13, 14, 12, 14, 15, // Cara trasera
            8, 12, 15, 8, 15, 11,   // Cara izquierda
            9, 13, 14, 9, 14, 10,   // Cara derecha
            8, 9, 13, 8, 13, 12,    // Cara superior
            11, 10, 14, 11, 14, 15  // Cara inferior
        };

        public Objeto()
        {
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        public void Dibujar(int shaderProgramObject, Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            GL.UseProgram(shaderProgramObject);
            GL.BindVertexArray(vertexArrayObject);

            // matrices de transformación a shaders
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgramObject, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgramObject, "view"), false, ref view);
            ////////
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgramObject, "projection"), false, ref projection);
            ////////

            // Dibujar objeto
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteBuffer(elementBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }
    }
}
