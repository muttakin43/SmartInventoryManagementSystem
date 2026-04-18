using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ISaleUnitOfWork _customerUnitOfWork;

        public CustomerService(ISaleUnitOfWork saleUnitOfWork)
        {
            _customerUnitOfWork = saleUnitOfWork;

        }

        public async Task<Result<int>> CreateAsync(CustomerCreateRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            await _customerUnitOfWork.CustomerRepository.AddAsync(customer);
            await _customerUnitOfWork.SaveChangesAsync();

            return Result<int>.SuccessResult(customer.id);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var customer = await _customerUnitOfWork.CustomerRepository.GetByIdAsync(id);

            if (customer == null)
                return Result<bool>.FailureResult("Customer not found");

            await _customerUnitOfWork.CustomerRepository.DeleteAsync(customer);
            await _customerUnitOfWork.SaveChangesAsync();

            return Result<bool>.SuccessResult(true);
        }

        public async Task<Result<IList<CustomerResponse>>> GetAllAsync()
        {
            var customers = await _customerUnitOfWork.CustomerRepository
                .GetAsync(x => x, null, null, null, true); 

            var data = customers.Select(x => new CustomerResponse
            {
                Id = x.id,
                Name = x.Name,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber
            }).ToList();

            return Result<IList<CustomerResponse>>.SuccessResult(data);
        }

        public async  Task<Result<CustomerResponse>> GetByIdAsync(int id)
        {
            var data = await _customerUnitOfWork.CustomerRepository
         .GetFirstorDefaultAsync(
             x => new CustomerResponse
             {
                 Id = x.id,
                 Name = x.Name,
                 Address = x.Address,
                 PhoneNumber = x.PhoneNumber
             },
             x => x.id == id
         );

            if (data == null)
                return Result<CustomerResponse>.FailureResult("Not found");

            return Result<CustomerResponse>.SuccessResult(data);
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerCreateRequest request)
        {
            var customer = await _customerUnitOfWork.CustomerRepository.GetByIdAsync(id);

            if (customer == null)
                return Result<bool>.FailureResult("Customer not found");

            customer.Name = request.Name;
            customer.Address = request.Address;
            customer.PhoneNumber = request.PhoneNumber;

            await _customerUnitOfWork.CustomerRepository.UpdateAsync(customer);
            await _customerUnitOfWork.SaveChangesAsync();

            return Result<bool>.SuccessResult(true);
        }
    }
}
