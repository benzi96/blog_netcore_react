/**
 *
 * App.js
 *
 * This component is the skeleton around the actual pages, and should only
 * contain code that should be seen on all pages. (e.g. navigation bar)
 *
 */

import React, { memo } from 'react';
import { Switch, Route } from 'react-router-dom';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { createStructuredSelector } from 'reselect';
import { compose } from 'redux';

import { useInjectSaga } from 'utils/injectSaga';
import { useInjectReducer } from 'utils/injectReducer';
import HomePage from 'containers/HomePage/Loadable';
import Dashboard from 'containers/Dashboard/Loadable';
import LoginPage from 'containers/LoginPage/Loadable';
import RegisterPage from 'containers/RegisterPage/Loadable';
import NotFoundPage from 'containers/NotFoundPage/Loadable';
import CallBackPage from 'containers/CallBackPage/Loadable';
import PostPage from 'containers/PostPage/Loadable';

import GlobalStyle from '../../global-styles';

import reducer from './reducer';
import saga from './saga';
import { getCategoryRequest } from './actions';

export function App({ dispatch }) {
  useInjectReducer({ key: 'global', reducer });
  useInjectSaga({ key: 'global', saga });

  React.useEffect(() => {
    dispatch(getCategoryRequest());
  }, []);

  return (
    <div>
      <Switch>
        <Route exact path="/login" component={LoginPage} />
        <Route path="/dashboard" component={Dashboard} />
        <Route exact path="/register" component={RegisterPage} />
        <Route exact path="/callback" component={CallBackPage} />
        <Route exact path="/post/:id/:slug" component={PostPage} />
        <Route exact path="/category/:name" component={HomePage} />
        <Route exact path="/" component={HomePage} />
        <Route component={NotFoundPage} />
      </Switch>
      <GlobalStyle />
    </div>
  );
}

App.propTypes = {
  dispatch: PropTypes.func.isRequired,
};

const mapStateToProps = createStructuredSelector({});

function mapDispatchToProps(dispatch) {
  return {
    dispatch,
  };
}

const withConnect = connect(
  mapStateToProps,
  mapDispatchToProps,
);

export default compose(
  withConnect,
  memo,
)(App);
