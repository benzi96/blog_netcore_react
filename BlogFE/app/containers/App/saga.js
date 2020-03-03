import { call, put, select, takeLatest } from 'redux-saga/effects';
import { get } from '../../api/axiosApi';
import { GET_CATEGORY_REQUEST } from './constants';

import { getCategorySuccess, getCategoryError } from './actions';

export default function* appSaga() {
  yield takeLatest(GET_CATEGORY_REQUEST, getCategorySaga);
}

export function* getCategorySaga() {
  try {
    const response = yield call(get, '/api/category');
    yield put(getCategorySuccess(response));
  } catch (error) {
    yield put(getCategoryError(error));
  }
}
