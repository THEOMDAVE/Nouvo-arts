# Nouvo Studio - Contemporary Art Gallery

A modern .NET MVC web application for showcasing contemporary art, featuring a complete admin panel for managing categories and artworks.

## Features

### Frontend
- **Home Page**: Hero section with featured artworks carousel and category teasers
- **Categories**: Browse art categories with filtering
- **Artworks**: View all artworks with search and filter capabilities
- **Artwork Details**: Detailed view with pricing and contact information
- **Responsive Design**: Mobile-first design with Bootstrap 5
- **Favorites**: Local storage-based favorites system

### Admin Panel
- **Dashboard**: Overview of categories and artworks count
- **Category Management**: Full CRUD operations for art categories
- **Artwork Management**: Full CRUD operations for artworks
- **Modern UI**: Clean admin interface with sidebar navigation

### Backend
- **.NET 8 MVC**: Modern web framework
- **Entity Framework Core**: Database ORM with SQL Server
- **Repository Pattern**: Service layer for business logic
- **Web API**: RESTful API endpoints for AJAX operations
- **Seed Data**: Pre-populated with sample categories and artworks

## Technology Stack

- **Backend**: .NET 8, ASP.NET Core MVC, Entity Framework Core
- **Database**: SQL Server LocalDB
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Icons**: Bootstrap Icons
- **Fonts**: Google Fonts (Playfair Display, Inter)

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore packages:
   ```bash
   dotnet restore
   ```

4. Update the database:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. Open your browser and navigate to `https://localhost:5001`

### Database Configuration

The application uses SQL Server LocalDB by default. The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NouvoStudioDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Project Structure

```
NouvoStudio/
├── Controllers/           # MVC Controllers
│   ├── HomeController.cs
│   ├── CategoriesController.cs
│   ├── ArtworksController.cs
│   ├── AdminController.cs
│   └── Api/              # Web API Controllers
├── Models/               # Data Models
│   ├── Category.cs
│   ├── Artwork.cs
│   ├── BlogPost.cs
│   └── ContactMessage.cs
├── Services/             # Business Logic
│   ├── ICategoryService.cs
│   ├── CategoryService.cs
│   ├── IArtworkService.cs
│   └── ArtworkService.cs
├── Data/                 # Data Access
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Views/                # Razor Views
│   ├── Home/
│   ├── Categories/
│   ├── Artworks/
│   ├── Admin/
│   └── Shared/
└── wwwroot/              # Static Files
    ├── css/
    └── js/
```

## API Endpoints

### Categories API
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

### Artworks API
- `GET /api/artworks` - Get all artworks
- `GET /api/artworks/{id}` - Get artwork by ID
- `GET /api/artworks/featured` - Get featured artworks
- `POST /api/artworks` - Create new artwork
- `PUT /api/artworks/{id}` - Update artwork
- `DELETE /api/artworks/{id}` - Delete artwork
- `POST /api/artworks/search` - Search artworks

## Features in Detail

### Search and Filtering
- Search artworks by name or code
- Filter by size (Small, Medium, Large)
- Filter by medium (Oil, Acrylic, Watercolor, Mixed Media)
- Category-based filtering

### Admin Panel
- Secure admin area (add authentication as needed)
- Category management with image uploads
- Artwork management with full details
- Dashboard with statistics

### Responsive Design
- Mobile-first approach
- Bootstrap 5 components
- Custom CSS for art gallery aesthetics
- Smooth animations and transitions

## Customization

### Adding New Categories
1. Use the admin panel at `/Admin/Categories`
2. Or add directly to the database
3. Categories automatically appear in the frontend

### Adding New Artworks
1. Use the admin panel at `/Admin/Artworks`
2. Set featured status for homepage display
3. Include pricing and detailed descriptions

### Styling
- Modify `wwwroot/css/site.css` for custom styles
- Update color scheme in CSS variables
- Add new Bootstrap components as needed

## Future Enhancements

- User authentication and authorization
- Shopping cart and e-commerce functionality
- Artist profiles and management
- Blog system for art news
- Image upload and management
- Email notifications
- Advanced search with filters
- Social media integration

## License

This project is created for educational and portfolio purposes.

## Support

For questions or issues, please create an issue in the repository or contact the development team.
