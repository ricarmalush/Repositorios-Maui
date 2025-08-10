# App de Control de Fichajes con .NET MAUI

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-6.0-green.svg)](https://dotnet.microsoft.com/en-us/apps/maui)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Una breve y atractiva descripción de tu proyecto.

Este proyecto es una **aplicación móvil multiplataforma para el control de fichajes de empleados**, desarrollada con **.NET MAUI**. Su objetivo es proporcionar una solución sencilla y eficiente para que los empleados registren su entrada y salida desde sus dispositivos móviles.

La aplicación se comunica con una API de backend para gestionar los registros de fichaje y otros datos relevantes, ofreciendo una experiencia de usuario fluida y segura.

---

## 🚀 Características Principales

* **Registro de fichaje intuitivo**: Interfaz simple para marcar la entrada y salida con un solo toque.
* **Geolocalización en fichajes**: Almacena la ubicación del dispositivo en el momento del fichaje para verificar la exactitud.
* **Visualización de historial**: Permite a los empleados ver su historial de fichajes.
* **Notificaciones**: Envía notificaciones para recordar a los empleados que fichen al llegar o salir.
* **Multiplataforma**: Compatible con iOS, Android y Windows desde una única base de código.

---

## 🛠️ Tecnologías y Arquitectura

* **Framework**: .NET MAUI
* **Lenguaje**: C#
* **IDE**: Visual Studio 2022
* **Plataformas**: iOS, Android, Windows
* **Patrones de diseño**: (Ej. MVVM - Model-View-ViewModel)
* **Integración**: La app se conecta a una API RESTful de backend para la gestión de datos.

---

## ⚙️ Configuración y Ejecución

Sigue estos pasos para ejecutar la aplicación en tu entorno local.

1.  **Requisitos previos**:
    * [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) (o la versión que uses)
    * [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) con la carga de trabajo de **"Desarrollo de IU de aplicaciones multiplataforma de .NET"**.
    * Configura el emulador o conecta un dispositivo para Android/iOS.

2.  **Clonar el repositorio**:
    ```bash
    git clone [https://github.com/ricarmalush/tu-repositorio-maui.git](https://github.com/ricarmalush/tu-repositorio-maui.git)
    cd tu-repositorio-maui
    ```

3.  **Restaurar dependencias**:
    ```bash
    dotnet restore
    ```

4.  **Configurar la conexión al backend**:
    * Asegúrate de que la URL de tu API de backend esté configurada correctamente en el archivo de configuración de la aplicación.

5.  **Ejecutar el proyecto**:
    * Desde la terminal, puedes ejecutar para la plataforma deseada:
        ```bash
        # Para Android
        dotnet build -t:Run -f net6.0-android
        # Para iOS (requiere un Mac en la red)
        dotnet build -t:Run -f net6.0-ios
        ```
    * Desde Visual Studio: Selecciona el emulador o dispositivo deseado y presiona `F5`.

---

## 🤝 Contribuciones

Agradecemos cualquier contribución para mejorar esta aplicación. Sigue las instrucciones de la sección de contribuciones en el `README.md` del repositorio de backend para el flujo de trabajo.

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.
