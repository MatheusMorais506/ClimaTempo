export default function api_options() {
  return {
    params:{
      Api_Url:process.env.REACT_APP_URL 
      //variavel de ambiente no ambiente de hospedagem (vercel por exemplo), 
      //essa variavel inclui a dominio da api no Azure
    }
  };
}