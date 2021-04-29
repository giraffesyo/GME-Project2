import type { AppProps /*, AppContext */ } from 'next/app'

function OverTheMoonWebApp({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />
}

export default OverTheMoonWebApp
