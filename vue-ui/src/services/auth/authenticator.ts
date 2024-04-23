import { isAxiosError } from 'axios'
import api from '@/services/api'

interface IAuthenticator {
  authenticate(username: string, password: string): Promise<string>
}

export class MyAuthenticator implements IAuthenticator {
  async authenticate(username: string, password: string): Promise<string> {
    try {
      const response = await api.post('/api/Account/signin', { username, password })

      if (response.status === 200) {
        // If the status code is 200, the authentication was successful.
        // Verify and decode the JWT from the server response.
        return response.data.token
      }
      else if (response.status === 404) {
        throw new Error('Username not found')
      }
      else if (response.status === 401) {
        throw new Error('Wrong password')
      }
      else {
        throw new Error('An error occurred during authentication')
      }
    }

    catch (error: unknown) {
      console.error(`there is some error to call API : ${error}`)
      if (isAxiosError(error)) {
        if (error.response?.status === 401)
          throw new Error('Wrong password')
        else if (error.response?.status === 404)
          throw new Error('Username not found')
        else
          throw new Error('An error occurred during authentication')
      }
      else {
        throw new TypeError('An unexpected error occurred')
      }
    }
  }
}
