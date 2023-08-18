using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDigital.Data;
using BancoDigital.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace BancoDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly APIDbContext _context;
    

        public ContasController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/Contas
        [HttpGet("Consulta Contas")]
        public async Task<ActionResult<IEnumerable<Conta>>> GetConta()
        {
          if (_context.Conta == null)
          {
              return NotFound();
          }
            return await _context.Conta.ToListAsync();
        }

        // GET: api/Contas
        [HttpGet("Consutas dividas")]
        public async Task<ActionResult<IEnumerable<Divida>>> GetDividas()
        {
            if (_context.Conta == null)
            {
                return NotFound();
            }
            return await _context.Dividas.ToListAsync();
        }

        // GET: api/Contas
        [HttpGet("Consutas transacoes")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTrasnsacoes()
        {
            if (_context.Transacaos == null)
            {
                return NotFound();
            }
            return await _context.Transacaos.ToListAsync();
        }

        // GET: api/Contas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conta>> GetConta(int id)
        {
          if (_context.Conta == null)
          {
              return NotFound("o campo conta não pode estar vazio!");
          }
            var conta = await _context.Conta.FindAsync(id);

            if (conta == null)
            {
                return NotFound("Conta não encontrado!");
            }

            return conta;
        }

        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Criar Conta")]
        public async Task<ActionResult<Conta>> PostConta(Conta conta)
        {
          if (_context.Conta == null)
          {
              return Problem("Entity set 'APIDbContext.Conta'  is null.");
          }
                  

            _context.Conta.Add(conta); 
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConta", new { id = conta.Id }, conta);
        }

        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Criar Divida")]
        public async Task<ActionResult<Conta>> CriarDivida(Divida divida)
        {
            if (_context.Dividas == null)
            {
                return Problem("Não existe divida cadastrada");
            }


            _context.Dividas.Add(divida); 
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDividas", new { id= divida.Id }, divida);
        }

        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("sacar")]
        public async Task<ActionResult<Conta>> Sacar(int contaId, decimal valor)
        {

            var conta = await _context.Conta.FindAsync(contaId);
            //var transacao = await _context.Transacaos.FindAsync(contaId);
            //var transacoes = await _context.Transacaos.AddAsync(Transacaos);

            if (conta == null)
            {
                return NotFound("Conta não encontrado!");
            }

            if (conta.Saldo < valor)
                return BadRequest("Saldo insuficiente");

            conta.Saldo -= valor;
            //_context.Conta.Update(conta);
            _context.Entry(conta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _context.Transacaos.Add(new Transacao { ContaId = contaId, Valor = -valor, Data = DateTime.Now });

            await _context.SaveChangesAsync();

            return Ok("Movimentação realizada com sucesso! Saldo da conta ficou em: " + conta.Saldo);


        }


        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("depositar")]
        public async Task<ActionResult<Conta>> Depositar(int contaId, decimal valor)
        {

            var conta = await _context.Conta.FindAsync(contaId);
            //var transacao = await _context.Transacaos.FindAsync(contaId);
            //var transacoes = await _context.Transacaos.AddAsync(Transacaos);

            if (conta == null)
            {
                return NotFound("Conta não encontrado!");
            }

            if (valor==0)
                return BadRequest("valor de deposito deve ser superior a 0 ");

            conta.Saldo += valor;
            //_context.Conta.Update(conta);
            _context.Entry(conta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _context.Transacaos.Add(new Transacao { ContaId = contaId, Valor = valor, Data = DateTime.Now });

            await _context.SaveChangesAsync();

            return Ok("Movimentação realizada com sucesso! Saldo da conta ficou em: " + conta.Saldo);


        }


        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("quitar-divida")]
        public async Task<ActionResult<Conta>> quitar(int contaId)
        {

            var divida = await _context.Dividas.FindAsync(contaId);
            var conta = await _context.Conta.FindAsync(contaId);
            //var transacao = await _context.Transacaos.FindAsync(contaId);
            //var transacoes = await _context.Transacaos.AddAsync(Transacaos);

            if (conta == null)
            {
                return NotFound("Conta não encontrada!");
            }

            if (divida == null)
            {
                return NotFound("Divida não encontrada para esta conta!");
            }

       

            if (conta.Saldo < divida.Valor)
                return BadRequest("Saldo insuficiente para quitar a dívida");

            //atualiza conta
            conta.Saldo -= divida.Valor;
            _context.Entry(conta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //Insere transacao
            _context.Transacaos.Add(new Transacao { ContaId = contaId, Valor = -divida.Valor, Data = DateTime.Now });
            await _context.SaveChangesAsync();

            //atualiza divida
            divida.Valor = 0;
            _context.Dividas.Remove(divida);
            await _context.SaveChangesAsync();


            return Ok("Divida quitada com sucesso!: " + conta.Saldo);


        }


    }
}
