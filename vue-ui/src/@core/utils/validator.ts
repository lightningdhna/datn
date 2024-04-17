export const requiredValidator = (value: string) => {
  return value !== '' || 'This field is required'
}

export const confirmedValidator = (value1: string, value2: string) => {
  return value1 === value2 || 'The values must match'
}

export const emailValidator = (email: string) => {
  const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

  return re.test(String(email).toLowerCase()) || 'Must be a valid email'
}

export const urlValidator = (url: string) => {
  const re = /^(ftp|http|https):\/\/[^ "]+$/

  return re.test(url) || 'Must be a valid URL'
}

export const passwordValidator = (password: string) => {
  if (password.length < 8)
    return 'Password must be at least 8 characters long'

  if (!/[a-z]/.test(password))
    return 'Password must contain at least one lowercase letter'

  if (!/[A-Z]/.test(password))
    return 'Password must contain at least one uppercase letter'

  if (!/\d/.test(password))
    return 'Password must contain at least one digit'

  if (!/[@$!%*?&]/.test(password))
    return 'Password must contain at least one special character (@, $, !, %, *, ?, &)'

  return true
}
