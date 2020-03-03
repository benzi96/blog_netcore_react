/**
 *
 * CallBackPage
 *
 */

import React, { memo } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { compose } from 'redux';
import { CallbackComponent } from 'redux-oidc';
import userManager from 'api/userManager';

export function CallBackPage({ history }) {
  return (
    <CallbackComponent
      userManager={userManager}
      successCallback={() => history.push('/')}
      errorCallback={error => {
        history.push('/');
        console.error(error);
      }}
    >
      <div>Redirecting...</div>
    </CallbackComponent>
  );
}

CallBackPage.propTypes = {
  dispatch: PropTypes.func.isRequired,
  history: PropTypes.object,
};

function mapDispatchToProps(dispatch) {
  return {
    dispatch,
  };
}

const withConnect = connect(
  null,
  mapDispatchToProps,
);

export default compose(
  withConnect,
  memo,
)(CallBackPage);
