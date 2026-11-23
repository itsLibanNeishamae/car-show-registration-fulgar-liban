# React + Vite

## Backend database (SQLite)

The API stores its SQLite database at `backend/Api/Data/carshow.db`. The API creates the `Data` directory if needed and applies pending EF Core migrations on startup. Existing database contents are preserved between restarts; the database file is excluded from source control.

Install the EF Core CLI once if it is not already installed:

```sh
dotnet tool install --global dotnet-ef --version 10.0.15
```

From the `backend` directory, the initial migration command is:

```sh
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api --output-dir Persistence/Migrations
```

The repository already includes `InitialCreate`, so do not run that command again unless you first remove/replace the existing migration. To apply the included migration to the local database:

```sh
dotnet ef database update --project Infrastructure --startup-project Api
```

For later schema changes, create a new migration with a new name, then run the database update command again.

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.
