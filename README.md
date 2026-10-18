# GymControl

Sistema de información de escritorio para la gestión de un gimnasio: miembros, entrenadores, tipos de membresía, membresías, pagos y asistencias.

Proyecto final del curso ISB-13 Programación II, Universidad Internacional San Isidro Labrador. Profesor: Ing. Joshua Granados Loría.

## Integrantes

Tiffany Mora Sánchez

## Descripción

GymControl centraliza en una base de datos relacional la información que un gimnasio pequeño suele llevar en cuadernos u hojas de cálculo. Permitirá registrar, consultar, buscar, modificar y eliminar información, validar los datos ingresados y calcular valores como la edad del miembro, los días restantes de la membresía, el saldo pendiente y el estado de cada membresía.

## Estado actual del proyecto (Avance 1)

- Base de datos diseñada y creada en SQL Server, con script y datos de prueba.
- Clases base de las entidades en la carpeta Logica, con herencia, encapsulamiento y valores calculados.
- Pendiente: formularios de Windows Forms, conexión con SQL Server, CRUD, validaciones y navegación.

## Tecnologías utilizadas

- Lenguaje: C#
- Interfaz: Windows Forms (.NET 10)
- Base de datos: Microsoft SQL Server (SQL Server Express)
- Herramientas: Visual Studio, SQL Server Management Studio (SSMS), Git y GitHub

## Requisitos para ejecutar el programa

- Windows
- Visual Studio con la carga de trabajo de desarrollo de escritorio con .NET
- SQL Server Express y SQL Server Management Studio

## Instalación de la base de datos

1. Abrir SQL Server Management Studio y conectarse al servidor local.
2. Abrir el archivo GymControl.sql (menú Archivo, Abrir, Archivo).
3. Ejecutarlo completo con F5.

El script crea la base de datos GymControl, sus seis tablas, las restricciones y los datos de prueba. Se puede ejecutar más de una vez, porque borra y vuelve a crear las tablas.

## Ejecución del proyecto

1. Abrir el archivo GymControl.slnx con Visual Studio.
2. Compilar con Ctrl + Shift + B.

## Credenciales de prueba

No aplica en esta etapa.

## Estructura del proyecto

- GymControl.sql: script de la base de datos.
- Logica: clases de las entidades (Persona, Miembro, Entrenador, TipoMembresia, Membresia, Pago y Asistencia).
- Form1.cs y Program.cs: formulario inicial y punto de entrada.
- GymControl_Avance1.docx: documento de avance.

## Herramientas adicionales

No se utilizan bibliotecas adicionales por el momento.

## Declaración del uso de inteligencia artificial generativa

- Herramienta: Claude (Anthropic).
- Uso: apoyo en el diseño de la base de datos, la documentación, la organización del proyecto y las clases base en C#.
- El estudiante eligió el tema, generó el diagrama, realizo con ayuda, ejecutó y verificó el script de la base de datos, creó y compiló el proyecto con sus clases, y administró el repositorio de GitHub.
