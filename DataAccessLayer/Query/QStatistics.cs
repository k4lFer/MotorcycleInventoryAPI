using DataAccessLayer.Connection;
using DataTransferLayer.Object;
using System;
using System.Linq;

namespace DataAccessLayer.Query
{
    public class QStatistics
    {
        public DtoStatistics GetStatistics()
        {
            using DataBaseContext dbc = new();

            // Fechas para el cálculo
            DateTime now = DateTime.UtcNow;
            DateTime startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfLastMonth = startOfThisMonth.AddMonths(-1);
            DateTime endOfLastMonth = startOfThisMonth.AddTicks(-1);

            // Total de motocicletas
            int motorcyclesTotal = dbc.Motorcyles.Count();

            // Nuevas motocicletas este mes
            int newMotorcyclesThisMonth = dbc.Motorcyles
                .Count(m => m.createdAt >= startOfThisMonth);

            // Porcentaje de cambio de motocicletas comparado al mes pasado
            int motorcyclesLastMonth = dbc.Motorcyles
                .Count(m => m.createdAt >= startOfLastMonth && m.createdAt <= endOfLastMonth);
            string percentageMotorcyclesForMonth = motorcyclesLastMonth > 0
                ? ((newMotorcyclesThisMonth - motorcyclesLastMonth) / (double)motorcyclesLastMonth * 100).ToString("F2") + "%"
                : "N/A";

            // Total de marcas
            int totalBrands = dbc.Brands.Count();

            // Nuevas marcas este mes
            int newBrandsThisMonth = dbc.Brands
                .Count(b => b.createdAt >= startOfThisMonth);

            // Total de modelos
            int totalModels = dbc.Types.Count();
            
            // Nuevos modelos este mes
            int newModelsThisMonth = dbc.Types
                .Count(b => b.createdAt >= startOfThisMonth);

            // Nuevos clientes este mes
            int totalCustomers = dbc.Users.Count();
            int newCustomersThisMonth = dbc.Users
                .Count(c => c.createdAt >= startOfThisMonth);
            int customersLastMonth = dbc.Users
                .Count(c => c.createdAt >= startOfLastMonth && c.createdAt <= endOfLastMonth);
            string customersGrowthPercentage = customersLastMonth > 0
                ? ((newCustomersThisMonth - customersLastMonth) / (double)customersLastMonth * 100).ToString("F2") + "%"
                : "N/A";

            // Ventas totales
            decimal totalSales = dbc.Sales.Sum(s => s.totalPrice);

            // Ventas del último mes
            decimal salesLastMonth = dbc.Sales
                .Where(s => s.date >= startOfLastMonth && s.date <= endOfLastMonth)
                .Sum(s => s.totalPrice);
            decimal salesThisMonth = dbc.Sales
                .Where(s => s.date >= startOfThisMonth)
                .Sum(s => s.totalPrice);
            string totalSalesGrowthPercentage = salesLastMonth > 0
                ? ((salesThisMonth - salesLastMonth) / salesLastMonth * 100).ToString("F2") + "%"
                : "N/A";

            // Crear y devolver el DTO
            return new DtoStatistics
            {
                motorcyclesTotal = motorcyclesTotal.ToString(),
                percentageMotorcyclesForMonth = percentageMotorcyclesForMonth,
                totalBrands = totalBrands.ToString(),
                newBrandsForMonth = newBrandsThisMonth.ToString(),
                totalModels = totalModels.ToString(),
                newModelsForMonth = newModelsThisMonth.ToString(),
                totalCustomers = totalCustomers.ToString(),
                customersGrowthPercentage = customersGrowthPercentage,
                totalSales = totalSales.ToString("F2"),
                totalSalesGrowthPercentage = totalSalesGrowthPercentage
            };
        }
    }
}